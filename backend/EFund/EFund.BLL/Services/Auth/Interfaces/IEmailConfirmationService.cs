using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Auth.Interfaces;

public interface IEmailConfirmationService
{
    Task<Either<ErrorDTO, AuthSuccessDTO>> ConfirmEmailAsync(ConfirmEmailDTO dto);
    Task<Option<ErrorDTO>> ResendConfirmationCodeAsync(ResendConfirmationCodeDTO dto);
}