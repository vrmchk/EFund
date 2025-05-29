using AutoMapper;
using EFund.Common.Models.DTO.Notification;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDTO>();
    }
}