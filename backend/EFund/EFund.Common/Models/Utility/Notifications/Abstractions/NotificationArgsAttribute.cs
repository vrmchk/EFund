namespace EFund.Common.Models.Utility.Notifications.Abstractions;

[AttributeUsage(AttributeTargets.Field)]
public class NotificationArgsAttribute(
    Type argsType
)
    : Attribute
{
    public Type ArgsType { get; } = argsType;
}