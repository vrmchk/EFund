using AutoMapper;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.DAL.Entities;
using EFund.Mapping.MappingActions;

namespace EFund.Mapping.Profiles;

public class FundraisingReportProfile : Profile
{
    public FundraisingReportProfile()
    {
        CreateMap<FundraisingReport, FundraisingReportDTO>()
            .ForMember(dest => dest.Attachments, opt => opt.MapAtRuntime())
            .AfterMap<FundraisingReportToFundraisingReportDTOMappingAction>();
        
        CreateMap<CreateFundraisingReportDTO, FundraisingReport>();
        CreateMap<UpdateFundraisingReportDTO, FundraisingReport>();
    }
}