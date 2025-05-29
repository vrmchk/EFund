using AutoMapper;
using EFund.Common.Models.DTO.Complaint;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class ComplaintProfile : Profile
{
    public ComplaintProfile()
    {
        CreateMap<Complaint, ComplaintDTO>();
        CreateMap<CreateComplaintDTO, Complaint>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => EFund.Common.Enums.ComplaintStatus.Pending))
            .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.Violations, opt => opt.Ignore());
    }
} 