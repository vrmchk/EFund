using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReport;
using LanguageExt;
using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Services.Interfaces;

public interface IFundraisingReportService
{
    Task<Either<ErrorDTO, FundraisingReportDTO>> AddAsync(Guid userId, CreateFundraisingReportDTO dto);
    Task<Either<ErrorDTO, FundraisingReportDTO>> UpdateAsync(Guid id, Guid userId, UpdateFundraisingReportDTO dto);
    Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId);
    Task<Option<ErrorDTO>> AddAttachmentsAsync(Guid reportId, Guid userId, IFormFileCollection files);
    Task<Option<ErrorDTO>> DeleteAttachmentsAsync(Guid reportId, Guid userId, DeleteAttachmentsDTO dto);
}