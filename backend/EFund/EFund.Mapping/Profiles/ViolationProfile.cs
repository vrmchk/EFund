using AutoMapper;
using EFund.Common.Models.DTO.Violation;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles
{
    public class ViolationProfile : Profile
    {
        public ViolationProfile()
        {
            CreateMap<Violation, ViolationDTO>();
            CreateMap<Violation, ViolationExtendedDTO>()
                .ForMember(dest => dest.GroupTitle, opt => opt.MapFrom(src => src.ViolationGroup.Title));

            CreateMap<ViolationGroup, ViolationGroupDTO>();
        }
    }
} 