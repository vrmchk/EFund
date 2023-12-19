using AutoMapper;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Common.Models.DTO.ReportAttachment;
using EFund.DAL.Entities;

namespace EFund.Mapping.MappingActions;

public class FundraisingReportToFundraisingReportDTOMappingAction : IMappingAction<FundraisingReport, FundraisingReportDTO>
{
    public void Process(FundraisingReport source, FundraisingReportDTO destination, ResolutionContext context)
    {
        destination.Attachments = context.Mapper.Map<List<ReportAttachmentDTO>>(source.Attachments);
    }
}