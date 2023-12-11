using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services.Auth.Auth;

public class RefreshTokenService : AuthServiceBase, IRefreshTokenService
{
    private readonly TokenValidationParameters _tokenValidationParameters;

    public RefreshTokenService(UserManager<User> userManager,
        JwtConfig jwtConfig,
        ILogger<AuthServiceBase> logger,
        TokenValidationParameters tokenValidationParameters)
        : base(userManager, jwtConfig, logger)
    {
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<Either<ErrorDTO, AuthSuccessDTO>> RefreshTokenAsync(RefreshTokenDTO dto)
    {
        var option = GetPrincipalFromToken(dto.AccessToken);
        return await option.MatchAsync(
            Some: async validatedToken =>
            {
                var expiryDateUnix =
                    long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(expiryDateUnix);

                if (expiryDateTimeUtc > DateTime.UtcNow)
                    return new IncorrectParametersErrorDTO("Access token is not expired yet");

                var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
                if (user is null)
                    return new IncorrectParametersErrorDTO("User with this id does not exist");

                if (DateTimeOffset.UtcNow > user.RefreshTokenExpiresAt)
                    return new ExpiredErrorDTO("Refresh token is expired");

                if (user.RefreshToken != dto.RefreshToken)
                    return new IncorrectParametersErrorDTO("Refresh token is invalid");

                return await GenerateAuthResultAsync(user);
            },
            None: () => new IncorrectParametersErrorDTO("Access token is invalid")
        );
    }

    private Option<ClaimsPrincipal> GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = _tokenValidationParameters.Clone();
        validationParameters.ValidateLifetime = false;
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return HasValidSecurityAlgorithm(validatedToken) ? principal : None;
        }
        catch
        {
            return None;
        }
    }

    private bool HasValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }
}