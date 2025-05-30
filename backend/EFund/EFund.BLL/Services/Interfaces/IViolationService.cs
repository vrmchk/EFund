using EFund.Common.Models.DTO.Violation;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IViolationService
{
    Task<List<ViolationGroupDTO>> GetGroupedViolationsAsync(bool withDeleted);
    Task<Either<ErrorDTO, ViolationExtendedDTO>> GetByIdAsync(Guid id);
}