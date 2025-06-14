﻿using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.User;
using LanguageExt;
using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Services.Interfaces;

public interface IUserService
{
    Task<Either<ErrorDTO, UserDTO>> GetByIdAsync(Guid id, string apiUrl, bool withNotifications);
    Task<Either<ErrorDTO, UserDTO>> UpdateUserAsync(Guid id, UpdateUserDTO dto, string apiUrl);
    Task<Option<ErrorDTO>> SendChangeEmailCodeAsync(Guid userId, ChangeEmailDTO dto);
    Task<Option<ErrorDTO>> ChangeEmailAsync(Guid userId, ConfirmChangeEmailDTO dto);
    Task<Option<ErrorDTO>> UploadAvatarAsync(Guid userId, IFormFile stream);
    Task<Option<ErrorDTO>> DeleteAvatarAsync(Guid userId);
    Task<Option<ErrorDTO>> MakeAdminAsync(MakeAdminDTO dto);
    Task<Option<ErrorDTO>> InviteAdminAsync(InviteAdminDTO dto);
    Task<Option<ErrorDTO>> PerformUserActionAsync(UserActionDTO dto);
    Task<Either<ErrorDTO, PagedListDTO<UserExtendedDTO>>> SearchAsync(SearchUserDTO dto, PaginationDTO pagination,
        string apiUrl, Guid providerId);
    Task<List<UserMinimizedDTO>> SearchMinimizedAsync(SearchUserDTO dto,
        string apiUrl);
}