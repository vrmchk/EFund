﻿namespace EFund.Common.Models.DTO.Auth;

public class ResetPasswordDTO
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}