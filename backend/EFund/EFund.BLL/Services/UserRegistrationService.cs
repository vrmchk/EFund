using EFund.BLL.Services.Interfaces;
using EFund.BLL.Utility;
using EFund.Common.Models.Configs;
using EFund.Common.Models.Utility;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IRepository<UserRegistration> _userRegistrationRepository;
    private readonly AuthConfig _authConfig;

    public UserRegistrationService(IRepository<UserRegistration> userRegistrationRepository, AuthConfig authConfig)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _authConfig = authConfig;
    }

    public async Task<Either<ErrorModel, int>> GenerateEmailConfirmationCodeAsync(Guid userId)
    {
        var existingRegistration = await _userRegistrationRepository.FirstOrDefaultAsync(r => r.UserId == userId);

        if (existingRegistration is not null)
            return new ErrorModel("You have already received a code");

        var code = TokenGenerator.GenerateNumericCode(_authConfig.ConfirmationCodeLenght);
        await _userRegistrationRepository.InsertAsync(new UserRegistration
        {
            Code = code,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.Add(_authConfig.ConfirmationCodeLifetime),
            IsCodeRegenerated = false,
            UserId = userId
        });

        return code;
    }

    public async Task<Option<ErrorModel>> CanConfirmEmailAsync(Guid userId, int code)
    {
        var registration = await _userRegistrationRepository
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.UserId == userId);

        if (registration is null)
            return new ErrorModel("You have not requested email confirmation");

        if (registration.Code != code)
            return new ErrorModel("Confirmation code is invalid");

        if (registration.ExpiresAt < DateTimeOffset.UtcNow)
            return new ErrorModel("Confirmation code has expired. Please request a new one");

        await _userRegistrationRepository.DeleteAsync(registration);
        return None;
    }

    public async Task<Either<ErrorModel, int>> RegenerateEmailConfirmationCodeAsync(Guid userId)
    {
        var registration = await _userRegistrationRepository.FirstOrDefaultAsync(r => r.UserId == userId);
        if (registration is null)
            return new ErrorModel("You have not requested email confirmation");

        if (registration.IsCodeRegenerated)
            return new ErrorModel("You have already requested a new code");

        registration.Code = TokenGenerator.GenerateNumericCode(_authConfig.ConfirmationCodeLenght);
        registration.ExpiresAt = DateTimeOffset.UtcNow.Add(_authConfig.ConfirmationCodeLifetime);
        registration.IsCodeRegenerated = true;
        await _userRegistrationRepository.UpdateAsync(registration);

        return registration.Code;
    }
}