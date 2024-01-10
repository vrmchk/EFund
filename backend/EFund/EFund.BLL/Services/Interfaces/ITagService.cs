using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Tag;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface ITagService
{
    Task<Either<ErrorDTO, TagDTO>> AddAsync(CreateTagDTO dto);
    Task<List<TagDTO>> GetByNameAsync(string name);
    Task<Option<ErrorDTO>> DeleteAsync(string name);
}