using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly IEmailSender _emailSender;

    public UserService(IMapper mapper,
        ILogger<UserService> logger,
        UserManager<User> userManager,
        IUserRegistrationService userRegistrationService,
        IEmailSender emailSender)
    {
        _mapper = mapper;
        _userManager = userManager;
        _userRegistrationService = userRegistrationService;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid userId, UpdateUserDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        _mapper.Map(dto, user);
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return new IdentityErrorDTO("Unable to update user. Please try again later");
        }

        _logger.LogInformation("User updated: {0}", user.Id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var either = await _userRegistrationService.GenerateEmailConfirmationCodeAsync(userId);
        return await either.Match<Task<Option<ErrorDTO>>>(
            Right: async code =>
            {
                var emailSent = await _emailSender.SendEmailAsync(dto.NewEmail,
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
            },
            Left: error =>
            {
                _logger.LogError("Unable to generate email confirmation code for user {0}", user.Id);
                return Task.FromResult<Option<ErrorDTO>>(new IncorrectParametersErrorDTO(error.Message));
            }
        );
    }

    public async Task<Option<ErrorDTO>> ChangeEmailAsync(Guid userId, ConfirmChangeEmailDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.Email == dto.NewEmail)
            return new IncorrectParametersErrorDTO("New email has to differ from the old one");

        var option = await _userRegistrationService.CanConfirmEmailAsync(userId, dto.Code);
        return await option.Match<Task<Option<ErrorDTO>>>(
            None: async () =>
            {
                var emailChanged = await _userManager.ChangeEmailAsync(user, dto.NewEmail);
                if (!emailChanged.Succeeded)
                {
                    _logger.LogIdentityErrors(user, emailChanged);
                    return new IdentityErrorDTO("Unable to change email. Please try again later");
                }

                _logger.LogInformation("Email changed for user {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to confirm email for user {0}", user.Id);
                return Task.FromResult<Option<ErrorDTO>>(new IncorrectParametersErrorDTO(error.Message));
            }
        );
    }
}