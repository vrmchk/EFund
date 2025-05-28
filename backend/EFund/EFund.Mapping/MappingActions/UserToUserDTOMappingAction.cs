using AutoMapper;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.User;
using EFund.DAL.Entities;
using Humanizer;

namespace EFund.Mapping.MappingActions;

public class UserToUserDTOMappingAction : IMappingAction<User, UserDTO>
{
    public void Process(User source, UserDTO destination, ResolutionContext context)
    {
        var badge = destination.Badges.FirstOrDefault(b => b.Type == BadgeType.UserSince);
        if (badge == null)
            return;

        var span = DateTimeOffset.UtcNow - source.CreatedAt;
        badge.Description += $" {span.Humanize()}";
    }
}