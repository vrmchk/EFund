using AutoMapper;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using Google.Apis.Auth;

namespace EFund.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SignUpDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        CreateMap<InviteAdminDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<GoogleJsonWebSignature.Payload, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GivenName));

        CreateMap<UpdateUserDTO, User>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null))
            .ForMember(dest => dest.HasMonobankToken, opt => opt.MapFrom(src => src.UserMonobanks.Count > 0))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath));

        CreateMap<User, UserExtendedDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null))
            .ForMember(dest => dest.HasMonobankToken, opt => opt.MapFrom(src => src.UserMonobanks.Count > 0))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath));
    }
}