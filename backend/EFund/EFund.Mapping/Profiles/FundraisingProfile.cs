using AutoMapper;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Fundraising;
using EFund.DAL.Entities;
using EFund.Mapping.MappingActions;

namespace EFund.Mapping.Profiles;

public class FundraisingProfile : Profile
{
    public FundraisingProfile()
    {
        CreateMap<Fundraising, FundraisingDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()))
            .ForMember(dest => dest.MonobankJarId, opt => opt.MapFrom(src => src.MonobankFundraising.JarId))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.DisplayName : null))
            .ForMember(dest => dest.UserAvatarUrl, opt => opt.MapFrom(src => src.User != null ? src.User.AvatarPath : null))
            .ForMember(dest => dest.Reports, opt => opt.MapAtRuntime());

        CreateMap<CreateFundraisingDTO, Fundraising>()
            .ForMember(dest => dest.MonobankFundraising, opt => opt.MapAtRuntime())
            .ForMember(dest => dest.Tags, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => FundraisingStatus.Open))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.Now))
            .AfterMap<CreateFundraisingDTOToFundraisingMappingAction>();

        CreateMap<UpdateFundraisingDTO, Fundraising>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore())
            .AfterMap<UpdateFundraisingDTOToFundraisingMappingAction>();

        CreateMap<UpdateFundraisingStatusDTO, Fundraising>()
            .ForMember(dest => dest.ClosedAt, opt => opt.MapFrom(src => src.Status == FundraisingStatus.Closed ? DateTimeOffset.Now : (DateTimeOffset?)null))
            .ForMember(dest => dest.ReadyForReviewAt, opt => opt.MapFrom(src => src.Status == FundraisingStatus.ReadyForReview ? DateTimeOffset.Now : (DateTimeOffset?)null));
    }
}