using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using LanguageExt;

namespace EFund.BLL.Services.Auth.Interfaces;

public interface IPasswordService
{
    Task<Option<ErrorDTO>> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto);
    Task<Option<ErrorDTO>> ForgotPasswordAsync(ForgotPasswordDTO dto);
    Task<Option<ErrorDTO>> ResetPasswordAsync(ResetPasswordDTO dto);
    Task<Option<ErrorDTO>> AddPasswordAsync(Guid userId, AddPasswordDTO dto);
}