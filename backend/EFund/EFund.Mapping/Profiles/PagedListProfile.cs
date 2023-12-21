using AutoMapper;
using EFund.BLL.Utility;
using EFund.Common.Models.DTO.Common;

namespace EFund.Mapping.Profiles;

public class PagedListProfile : Profile
{
    public PagedListProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedListDTO<>))
            .ForMember("Items", opt => opt.MapFrom(src => src));
    }
}