using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Auth.Interfaces;

public interface IRefreshTokenService
{
    Task<Either<ErrorDTO, AuthSuccessDTO>> RefreshTokenAsync(RefreshTokenDTO dto);
}