using System.Web;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services.Auth.Auth;

public class PasswordService : AuthServiceBase, IPasswordService
{
    private readonly IEmailSender _emailSender;
    private readonly CallbackUrisConfig _callbackUrisConfig;

    public PasswordService(UserManager<User> userManager,
        JwtConfig jwtConfig,
        ILogger<AuthServiceBase> logger,
        IEmailSender emailSender,
        CallbackUrisConfig callbackUrisConfig)
        : base(userManager, jwtConfig, logger)
    {
        _emailSender = emailSender;
        _callbackUrisConfig = callbackUrisConfig;
    }

    public async Task<Option<ErrorDTO>> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
        if (!isValidPassword)
            return new IncorrectParametersErrorDTO("Old password is incorrect");

        var isSamePassword = await _userManager.CheckPasswordAsync(user, dto.NewPassword);
        if (isSamePassword)
            return new IncorrectParametersErrorDTO("New password have to differ from the old one");

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            _logger.LogIdentityErrors(user, result);
            return new IdentityErrorDTO("Unable to change password");
        }

        var emailSent = await _emailSender.SendEmailAsync(user.Email!,
            new PasswordChangedMessage { UserName = user.DisplayName });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Password changed email sent to user: {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send email to user: {0}", user.Id);
                return new ExternalErrorDTO(error.Message);
            }
        );
    }

    public async Task<Option<ErrorDTO>> ForgotPasswordAsync(ForgotPasswordDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return new NotFoundErrorDTO("User with this email does not exist");

        if (!user.EmailConfirmed)
            return new IncorrectParametersErrorDTO("Your email is not confirmed yet");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var callbackUri = string.Format(_callbackUrisConfig.ResetPasswordUriTemplate, user.Email, encodedToken);

        var emailSent = await _emailSender.SendEmailAsync(user.Email!,
            new ResetPasswordMessage { UserName = user.DisplayName, ResetPasswordUri = callbackUri });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Forgot password email sent to user: {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send email to user: {0}", user.Id);
                return new ExternalErrorDTO(error.Message);
            }
        );
    }

    public async Task<Option<ErrorDTO>> ResetPasswordAsync(ResetPasswordDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return new NotFoundErrorDTO("User with this email does not exist");

        if (!user.EmailConfirmed)
            return new IncorrectParametersErrorDTO("Your email is not confirmed yet");

        var isSamePassword = await _userManager.CheckPasswordAsync(user, dto.NewPassword);
        if (isSamePassword)
            return new IncorrectParametersErrorDTO("New password have to differ from the old one");

        var decodedToken = HttpUtility.UrlDecode(dto.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
        if (!result.Succeeded)
        {
            _logger.LogIdentityErrors(user, result);
            return new IdentityErrorDTO("Unable to reset password");
        }

        var emailSent = await _emailSender.SendEmailAsync(user.Email!,
            new PasswordChangedMessage { UserName = user.DisplayName });

        return emailSent.Match<Option<ErrorDTO>>(
            None: () =>
            {
                _logger.LogInformation("Password changed email sent to user: {0}", user.Id);
                return None;
            },
            Some: error =>
            {
                _logger.LogError("Unable to send email to user: {0}", user.Id);
                return new ExternalErrorDTO(error.Message);
            }
        );
    }

    public async Task<Option<ErrorDTO>> AddPasswordAsync(Guid userId, AddPasswordDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (user.PasswordHash is not null)
            return new IncorrectParametersErrorDTO("You already have a password");

        var result = await _userManager.AddPasswordAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            _logger.LogIdentityErrors(user, result);
            return new IdentityErrorDTO("Unable to add password");
        }

        _logger.LogInformation("User {0} added password", user.Id);
        return None;
    }
}