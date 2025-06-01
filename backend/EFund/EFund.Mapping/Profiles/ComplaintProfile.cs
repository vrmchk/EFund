using AutoMapper;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Complaint;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class ComplaintProfile : Profile
{
    public ComplaintProfile()
    {
        CreateMap<Complaint, ComplaintDTO>()
            .ForMember(dest => dest.RequestedByUserName, opt => opt.MapFrom(src => src.RequestedByUser != null ? src.RequestedByUser.DisplayName : null))
            .ForMember(dest => dest.RequestedForUserName, opt => opt.MapFrom(src => src.RequestedForUser != null ? src.RequestedForUser.DisplayName : null))
            .ForMember(dest => dest.ReviewedByUserName, opt => opt.MapFrom(src => src.ReviewedByUser != null ? src.ReviewedByUser.DisplayName : null))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => $"#{src.Number:D6}"));

        CreateMap<CreateComplaintDTO, Complaint>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ComplaintStatus.Pending))
            .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.Violations, opt => opt.Ignore());
    }
} 