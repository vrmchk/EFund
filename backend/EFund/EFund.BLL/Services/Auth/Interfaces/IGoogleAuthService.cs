using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Auth.Interfaces;

public interface IGoogleAuthService
{
    Task<Either<ErrorDTO, AuthSuccessDTO>> SignUpAsync(string authorizationCode);
    Task<Either<ErrorDTO, AuthSuccessDTO>> SignInAsync(string authorizationCode);
}