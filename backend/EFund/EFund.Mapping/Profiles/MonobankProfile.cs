using AutoMapper;
using EFund.Client.Monobank.Models.Responses;
using EFund.Common.Models.DTO.Monobank;
using ISO._4217;

namespace EFund.Mapping.Profiles;

public class MonobankProfile : Profile
{
    public MonobankProfile()
    {
        CreateMap<Jar, JarDTO>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance / 100))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal / 100))
            .ForMember(dest => dest.CurrencyCode,
                opt => opt.MapFrom(src => CurrencyCodesResolver.GetCodeByNumber(src.CurrencyCode)));
    }
}