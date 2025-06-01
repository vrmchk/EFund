namespace EFund.Common.Models.DTO.User;

public class UserMinimizedDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Rating { get; set; }
}