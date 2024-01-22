using System.Web;
using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EFund.BLL.Services.Auth.Auth;

public class AuthService : AuthServiceBase, IAuthService
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly IUserRegistrationService _userRegistrationService;

    public AuthService(IMapper mapper,
        ILogger<AuthService> logger,
        UserManager<User> userManager,
        JwtConfig jwtConfig,
        IEmailSender emailSender,
        IUserRegistrationService userRegistrationService)
        : base(userManager, jwtConfig, logger)
    {
        _mapper = mapper;
        _emailSender = emailSender;
        _userRegistrationService = userRegistrationService;
    }

    public async Task<Either<ErrorDTO, SignUpResponseDTO>> SignUpAsync(SignUpDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is { CreatedByAdmin: false })
            return new AlreadyExistsErrorDTO("User with this email already exists");

        if (user != null)
        {
            var error = await UpdateUserCreateByAdmin(user, dto);
            if (error != null)
                return error;
        }
        else
        {
            user = _mapper.Map<User>(dto);
            var createdUser = await _userManager.CreateAsync(user, dto.Password);
            if (!createdUser.Succeeded)
            {
                _logger.LogIdentityErrors(user, createdUser);
                return new IdentityErrorDTO(
                    "Unable to create a user. Please try again later or use another email address");
            }
        }

        var role = dto.AdminToken != null ? Roles.Admin : Roles.User;
        var roleAdded = await _userManager.AddToRoleAsync(user, role);
        if (!roleAdded.Succeeded)
        {
            _logger.LogIdentityErrors(user, roleAdded);
            return new IdentityErrorDTO("Unable to create a user. Please try again later");
        }

        var generatedCode = await _userRegistrationService.GenerateEmailConfirmationCodeAsync(user.Id);
        return await generatedCode.Match<Task<Either<ErrorDTO, SignUpResponseDTO>>>(
            Right: async code =>
            {
                var emailSent = await _emailSender.SendEmailAsync(user.Email!,
                    new EmailConfirmationMessage { Code = code });

                return emailSent.Match<Either<ErrorDTO, SignUpResponseDTO>>(
                    None: () =>
                    {
                        _logger.LogInformation("Email confirmation code sent to user {0}", user.Id);
                        return new SignUpResponseDTO { UserId = user.Id };
                    },
                    Some: error =>
                    {
                        _logger.LogError("Unable to send email confirmation code to user {0}", user.Id);
                        return new ExternalErrorDTO(error.Message);
                    });
            },
            Left: error =>
            {
                _logger.LogError("Unable to generate email confirmation code for user {0}, error: {1}", user.Id,
                    error.Message);

                return Task.FromResult<Either<ErrorDTO, SignUpResponseDTO>>(
                    new IncorrectParametersErrorDTO(error.Message));
            }
        );
    }

    public async Task<Either<ErrorDTO, AuthSuccessDTO>> SignInAsync(SignInDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return new NotFoundErrorDTO("User with this email does not exist");

        if (!user.EmailConfirmed)
            return new IncorrectParametersErrorDTO("Confirm your email before signing in");

        if (user.IsBlocked)
            return new NotFoundErrorDTO("User blocked by administrators");

        var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!validPassword)
            return new IncorrectParametersErrorDTO("Email or password is incorrect");

        return await GenerateAuthResultAsync(user);
    }

    private async Task<ErrorDTO?> UpdateUserCreateByAdmin(User user, SignUpDTO dto)
    {
        _mapper.Map(dto, user);
        user.CreatedByAdmin = false;
        if (dto.AdminToken == null || !await _userManager.CanMakeAdminAsync(user, dto.AdminToken!))
            return new IncorrectParametersErrorDTO("Invalid admin token");

        var updatedUser = await _userManager.UpdateAsync(user);
        if (!updatedUser.Succeeded)
        {
            _logger.LogIdentityErrors(user, updatedUser);
            return new IdentityErrorDTO("Unable to create a user. Please try again later or use another email address");
        }

        var passwordAdded = await _userManager.AddPasswordAsync(user, dto.Password);
        if (!passwordAdded.Succeeded)
        {
            _logger.LogIdentityErrors(user, passwordAdded);
            return new IdentityErrorDTO("Unable to create a user. Please try again later or use another email address");
        }

        return null;
    }
}