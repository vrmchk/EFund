using System.Text.Json;
using AutoMapper;
using EFund.BLL.Extensions;
using EFund.Common.Models.DTO.Notification;
using EFund.Common.Models.Utility.Notifications.Abstractions;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDTO>()
            .ForMember(dest => dest.Args, opt => opt.MapFrom(src => MapArgs(src)));
    }

    private object? MapArgs(Notification src)
    {
        if (src.Args == null)
            return null;

        var argsAttribute = src.Reason.GetAttribute<NotificationArgsAttribute>();
        if (argsAttribute == null)
            return null;

        return JsonSerializer.Deserialize(src.Args, argsAttribute.ArgsType);
    }
}