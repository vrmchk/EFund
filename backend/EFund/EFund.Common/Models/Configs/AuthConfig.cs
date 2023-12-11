using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class AuthConfig : ConfigBase
{
    public int ConfirmationCodeLenght { get; set; }
    public TimeSpan ConfirmationCodeLifetime { get; set; }
    public string ResetPasswordUriTemplate { get; set; } = string.Empty;
}