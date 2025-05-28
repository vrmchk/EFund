using System.Web;
using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.Common.Constants;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using EFund.Hangfire.Jobs;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFund.BLL.Services.Auth.Auth;

public class GoogleAuthService : AuthServiceBase, IGoogleAuthService
{
    private readonly GoogleConfig _googleConfig;
    private readonly IRepository<UserRegistration> _userRegistrationRepository;
    private readonly IMapper _mapper;
    private readonly IHangfireService _hangfireService;

    public GoogleAuthService(
        UserManager<User> userManager,
        IRepository<UserRegistration> userRegistrationRepository,
        JwtConfig jwtConfig,
        GoogleConfig googleConfig,
        IMapper mapper,
        ILogger<GoogleAuthService> logger, 
        IHangfireService hangfireService)
        : base(userManager, jwtConfig, logger)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _googleConfig = googleConfig;
        _mapper = mapper;
        _hangfireService = hangfireService;
    }

    public async Task<Either<ErrorDTO, AuthSuccessDTO>> SignUpAsync(string authorizationCode, GoogleSingUpDTO dto)
    {
        var payload = await GetGooglePayloadAsync(authorizationCode);

        var user = await _userManager.FindByEmailAsync(payload.Email);
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
            user = _mapper.Map<User>(payload);

            user.EmailConfirmed = true;
            var createdUserResult = await _userManager.CreateAsync(user);

            if (!createdUserResult.Succeeded)
            {
                _logger.LogIdentityErrors(user, createdUserResult);
                return new IdentityErrorDTO("Unable to authenticate given user");
            }            
        }

        var role = dto.AdminToken != null ? Roles.Admin : Roles.User;
        var roleAdded = await _userManager.AddToRoleAsync(user, role);
        if (!roleAdded.Succeeded)
        {
            _logger.LogIdentityErrors(user, roleAdded);
            return new IdentityErrorDTO("Unable to create a user. Please try again later");
        }

        _hangfireService.Enqueue<AssignUserCreatedBadgesJob, AssignUserCreatedBadgesJobArgs>(
            new AssignUserCreatedBadgesJobArgs { UserId = user.Id });

        return await GenerateAuthResultAsync(user);
    }

    public async Task<Either<ErrorDTO, AuthSuccessDTO>> SignInAsync(string authorizationCode)
    {
        var payload = await GetGooglePayloadAsync(authorizationCode);
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
            return new NotFoundErrorDTO("Unable to find such user");

        if (!user.EmailConfirmed)
        {
            user.EmailConfirmed = true;
            var updatedUser = await _userManager.UpdateAsync(user);

            var userRegistration = await _userRegistrationRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

            if (userRegistration != null)
                await _userRegistrationRepository.DeleteAsync(userRegistration);

            if (!updatedUser.Succeeded)
            {
                _logger.LogIdentityErrors(user, updatedUser);
                return new IdentityErrorDTO("Unable to authenticate given user");
            }
        }

        return await GenerateAuthResultAsync(user);
    }

    private async Task<GoogleJsonWebSignature.Payload> GetGooglePayloadAsync(string authorizationCode)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleConfig.ClientId,
                ClientSecret = _googleConfig.ClientSecret
            }
        });

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            string.Empty,
            authorizationCode,
            _googleConfig.RedirectUri,
            CancellationToken.None);

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleConfig.ClientId },
            IssuedAtClockTolerance = _googleConfig.ClockTolerance,
            ExpirationTimeClockTolerance = _googleConfig.ClockTolerance,
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, settings);
        return payload;
    }

    private async Task<ErrorDTO?> UpdateUserCreateByAdmin(User user, GoogleSingUpDTO dto)
    {
        _mapper.Map(dto, user);
        user.CreatedByAdmin = false;
        user.EmailConfirmed = true;
        var decodedToken = HttpUtility.UrlDecode(dto.AdminToken);
        if (dto.AdminToken == null || !await _userManager.CanMakeAdminAsync(user, decodedToken!))
            return new IncorrectParametersErrorDTO("Invalid admin token");

        var updatedUser = await _userManager.UpdateAsync(user);
        if (!updatedUser.Succeeded)
        {
            _logger.LogIdentityErrors(user, updatedUser);
            return new IdentityErrorDTO("Unable to create a user. Please try again later or use another email address");
        }

        return null;
    }
}
