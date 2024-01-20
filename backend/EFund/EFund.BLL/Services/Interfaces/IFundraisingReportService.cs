using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Common.Models.DTO.ReportAttachment;
using LanguageExt;
using Microsoft.AspNetCore.Http;

namespace EFund.BLL.Services.Interfaces;

public interface IFundraisingReportService
{
    Task<Either<ErrorDTO, FundraisingReportDTO>> GetByIdAsync(Guid id, Guid userId, string apiUrl);
    Task<Either<ErrorDTO, FundraisingReportDTO>> AddAsync(Guid userId, CreateFundraisingReportDTO dto);
    Task<Either<ErrorDTO, FundraisingReportDTO>> UpdateAsync(Guid id, Guid userId, UpdateFundraisingReportDTO dto, string apiUrl);
    Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId);
    Task<Option<ErrorDTO>> AddAttachmentsAsync(Guid reportId, Guid userId, IFormFileCollection files);
    Task<Option<ErrorDTO>> UpdateAttachmentAsync(Guid reportId, Guid attachmentId, Guid userId, UpdateAttachmentDTO dto);
    Task<Option<ErrorDTO>> DeleteAttachmentsAsync(Guid reportId, Guid userId, DeleteAttachmentsDTO dto);
}