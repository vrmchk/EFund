﻿namespace EFund.Common.Models.DTO.Auth;

public class SignInDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}