﻿namespace EFund.Common.Models.DTO.User;

public class UserExtendedDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public bool HasPassword { get; set; }
    public bool HasMonobankToken { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool CreatedByAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public List<string> Roles { get; set; } = new();
}