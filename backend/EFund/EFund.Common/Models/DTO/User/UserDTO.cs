namespace EFund.Common.Models.DTO.User;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool HasPassword { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
}