﻿namespace EFund.Common.Models.DTO.Auth;

public class SignUpDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? AdminToken { get; set; }
}