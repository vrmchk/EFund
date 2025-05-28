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
            CreateMap<ViolationGroup, ViolationGroupDTO>();
        }
    }
} 