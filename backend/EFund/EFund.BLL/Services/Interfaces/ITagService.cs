using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Tag;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface ITagService
{
    Task<List<TagDTO>> GetAllAsync(PaginationDTO pagination);
    Task<Either<ErrorDTO, TagDTO>> AddAsync(CreateTagDTO dto);
    Task<List<TagDTO>> GetByNameAsync(string name, PaginationDTO pagination);
    Task<Option<ErrorDTO>> DeleteAsync(string name);
}