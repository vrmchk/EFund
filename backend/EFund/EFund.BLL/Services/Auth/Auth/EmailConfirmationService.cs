using EFund.BLL.Extensions;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services.Auth.Auth;

public class EmailConfirmationService : AuthServiceBase, IEmailConfirmationService
{
    private readonly IEmailSender _emailSender;
    private readonly IUserRegistrationService _userRegistrationService;

    public EmailConfirmationService(UserManager<User> userManager,
        JwtConfig jwtConfig,
        ILogger<AuthServiceBase> logger,
        IEmailSender emailSender,
        IUserRegistrationService userRegistrationService)
        : base(userManager, jwtConfig, logger)
    {
        _emailSender = emailSender;
        _userRegistrationService = userRegistrationService;
    }

    public async Task<Either<ErrorDTO, AuthSuccessDTO>> ConfirmEmailAsync(ConfirmEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.EmailConfirmed)
            return new IncorrectParametersErrorDTO("Email is already confirmed");

        var option = await _userRegistrationService.CanConfirmEmailAsync(dto.UserId, dto.Code);
        return await option.Match<Task<Either<ErrorDTO, AuthSuccessDTO>>>(
            None: async () =>
            {
                user.EmailConfirmed = true;
                var userUpdated = await _userManager.UpdateAsync(user);
                if (!userUpdated.Succeeded)
                {
                    _logger.LogIdentityErrors(user, userUpdated);
                    return new IdentityErrorDTO("Unable to confirm email. Please try again later");
                }

                _logger.LogInformation("Email confirmed for user: {0}", user.Id);
                return await GenerateAuthResultAsync(user);
            },
            Some: error =>
            {
                _logger.LogError("Unable to confirm email for user: {0}, error: {1}", user.Id, error.Message);
                return Task.FromResult<Either<ErrorDTO, AuthSuccessDTO>>(
                    new IncorrectParametersErrorDTO(error.Message));
            });
    }

    public async Task<Option<ErrorDTO>> ResendConfirmationCodeAsync(ResendConfirmationCodeDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var code = await _userRegistrationService.RegenerateEmailConfirmationCodeAsync(dto.UserId);
        var emailSent = await _emailSender.SendEmailAsync(user.Email!,
            new EmailConfirmationMessage { Code = code });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Email confirmation code sent to user {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send email confirmation code to user {0}", user.Id);
                return new ExternalErrorDTO(error.Message);
            }
        );
    }
}