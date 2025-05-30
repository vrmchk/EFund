using AutoMapper;
using EFund.Common.Models.DTO.FundraisingReview;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class FundraisingReviewProfile : Profile
{
    public FundraisingReviewProfile()
    {
        CreateMap<CreateFundraisingReviewDTO, FundraisingReview>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.Now));

        CreateMap<UpdateFundraisingReviewDTO, FundraisingReview>();

        CreateMap<FundraisingReview, FundraisingReviewDTO>();
    }   
}