using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IUserService
{
    Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id, string apiUrl);
    Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid userId, UpdateUserDTO dto, string apiUrl);
    Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto);
    Task<Option<ErrorDTO>> ChangeEmailAsync(Guid userId, ConfirmChangeEmailDTO dto);
    Task<Option<ErrorDTO>> UploadAvatarAsync(Guid userId, Stream stream, string fileContentType);
}