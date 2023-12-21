using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Fundraising;
using LanguageExt;
using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Services.Interfaces;

public interface IFundraisingService
{
    Task<Either<ErrorDTO, PagedListDTO<FundraisingDTO>>> Search(SearchFundraisingDTO dto, PaginationDTO pagination,
        string apiUrl);
    Task<Either<ErrorDTO, FundraisingDTO>> GetByIdAsync(Guid id, string apiUrl);
    Task<Either<ErrorDTO, FundraisingDTO>> AddAsync(Guid userId, CreateFundraisingDTO dto, string apiUrl);
    Task<Either<ErrorDTO, FundraisingDTO>> UpdateAsync(Guid id, Guid userId, UpdateFundraisingDTO dto, string apiUrl);
    Task<Option<ErrorDTO>> DeleteAsync(Guid id);
    Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId);
    Task<Option<ErrorDTO>> UploadAvatarAsync(Guid id, Guid userId, IFormFile file);
    Task<Option<ErrorDTO>> DeleteAvatarAsync(Guid id, Guid userId);
}