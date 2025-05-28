using AutoMapper;
using EFund.Common.Models.DTO.Auth;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using EFund.Mapping.MappingActions;
using Google.Apis.Auth;

namespace EFund.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SignUpDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.Now));

        CreateMap<InviteAdminDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.Now));

        CreateMap<GoogleJsonWebSignature.Payload, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GivenName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.Now));

        CreateMap<UpdateUserDTO, User>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null))
            .ForMember(dest => dest.HasMonobankToken, opt => opt.MapFrom(src => src.UserMonobanks.Count > 0))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath))
            .AfterMap<UserToUserDTOMappingAction>();

        CreateMap<User, UserExtendedDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null))
            .ForMember(dest => dest.HasMonobankToken, opt => opt.MapFrom(src => src.UserMonobanks.Count > 0))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath));
    }
}