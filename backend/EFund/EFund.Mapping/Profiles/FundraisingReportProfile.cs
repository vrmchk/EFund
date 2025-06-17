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
        
        CreateMap<CreateFundraisingReportDTO, FundraisingReport>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));

        CreateMap<UpdateFundraisingReportDTO, FundraisingReport>();
    }
}