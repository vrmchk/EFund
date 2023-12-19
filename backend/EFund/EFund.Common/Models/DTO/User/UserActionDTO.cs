namespace EFund.Common.Models.DTO.User;

public class UserActionDTO
{
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
}