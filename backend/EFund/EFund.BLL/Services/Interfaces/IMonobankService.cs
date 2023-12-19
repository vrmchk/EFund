using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Monobank;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IMonobankService
{
    Task<Option<ErrorDTO>> AddOrUpdateMonobankTokenAsync(Guid userId, string monobankToken);
    Task<Either<ErrorDTO, List<JarDTO>>> GetJarsAsync(Guid userId);
    Task<Either<ErrorDTO, List<JarDTO>>> GetJarsAsync(List<Guid> userIds);
    Task<Either<ErrorDTO, JarDTO>> GetJarByIdAsync(Guid userId, string jarId);
}