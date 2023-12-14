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
            .ForMember(dest => dest.CurrencyCode,
                opt => opt.MapFrom(src => CurrencyCodesResolver.GetCodeByNumber(src.CurrencyCode)));
    }
}