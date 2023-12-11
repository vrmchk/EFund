using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Auth.Interfaces;

public interface IAuthService
{
    Task<Either<ErrorDTO, SignUpResponseDTO>> SignUpAsync(SignUpDTO dto);
    Task<Either<ErrorDTO, AuthSuccessDTO>> SignInAsync(SignInDTO dto);
}