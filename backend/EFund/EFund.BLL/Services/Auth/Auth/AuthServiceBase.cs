using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EFund.BLL.Extensions;
using EFund.BLL.Utility;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EFund.BLL.Services.Auth.Auth;

public abstract class AuthServiceBase
{
    protected readonly JwtConfig _jwtConfig;
    protected readonly UserManager<User> _userManager;
    protected readonly ILogger<AuthServiceBase> _logger;

    protected AuthServiceBase(UserManager<User> userManager, JwtConfig jwtConfig, ILogger<AuthServiceBase> logger)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
        _logger = logger;
    }

    protected async Task<Either<ErrorDTO, AuthSuccessDTO>> GenerateAuthResultAsync(User user)
    {
        var option = await GenerateRefreshTokenAsync(user);
        return option.Match<Either<ErrorDTO, AuthSuccessDTO>>(
            Right: refreshToken => new AuthSuccessDTO
            {
                AccessToken = GenerateJwtToken(user),
                RefreshToken = refreshToken
            },
            Left: errorCode => new ErrorDTO(errorCode, "Unable to generate refresh token")
        );
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.Add(_jwtConfig.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }

    private async Task<Either<ErrorCode, string>> GenerateRefreshTokenAsync(User user)
    {
        user.RefreshToken = TokenGenerator.GenerateToken();
        user.RefreshTokenExpiresAt = DateTimeOffset.UtcNow.Add(_jwtConfig.RefreshTokenLifetime);
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
        {
            _logger.LogIdentityErrors(user, userUpdated);
            return ErrorCode.IdentityError;
        }

        return user.RefreshToken;
    }
}