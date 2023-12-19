using AutoMapper;
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
            .ForMember(dest => dest.Reports, opt => opt.MapAtRuntime());

        CreateMap<CreateFundraisingDTO, Fundraising>()
            .ForMember(dest => dest.MonobankFundraising, opt => opt.MapAtRuntime());

        CreateMap<UpdateFundraisingDTO, Fundraising>()
            .AfterMap<UpdateFundraisingDTOToFundraisingMappingAction>();
    }
}