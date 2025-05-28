using AutoMapper;
using EFund.BLL.Extensions;
using EFund.Common.Models.DTO.Badge;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class BadgeProfile : Profile
{
    public BadgeProfile()
    {
        CreateMap<Badge, BadgeDTO>()
            .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Type.GetDescription()));
    }
}