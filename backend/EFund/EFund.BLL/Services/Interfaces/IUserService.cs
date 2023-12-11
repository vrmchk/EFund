using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IUserService
{
    Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id);
    Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid userId, UpdateUserDTO dto);
    Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto);
    Task<Option<ErrorDTO>> ChangeEmailAsync(Guid userId, ConfirmChangeEmailDTO dto);
}