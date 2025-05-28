using EFund.Common.Models.DTO.Violation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFund.BLL.Services.Interfaces
{
    public interface IViolationService
    {
        Task<List<ViolationGroupDTO>> GetGroupedViolationsAsync(bool withDeleted);
    }
} 