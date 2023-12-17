using AutoMapper;
using EFund.Common.Models.DTO.Tag;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDTO>();
        CreateMap<CreateTagDTO, Tag>();
    }
}