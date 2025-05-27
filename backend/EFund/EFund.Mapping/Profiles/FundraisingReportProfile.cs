using AutoMapper;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class FundraisingReportProfile : Profile
{
    public FundraisingReportProfile()
    {
        CreateMap<FundraisingReport, FundraisingReportDTO>()
            .ForMember(dest => dest.Attachments, opt => opt.MapAtRuntime());
        
        CreateMap<CreateFundraisingReportDTO, FundraisingReport>();
        CreateMap<UpdateFundraisingReportDTO, FundraisingReport>();
    }
}