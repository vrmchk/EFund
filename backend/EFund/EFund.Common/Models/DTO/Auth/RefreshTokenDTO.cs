﻿namespace EFund.Common.Models.DTO.Auth;

public class RefreshTokenDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}