using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class CallbackUrisConfig : ConfigBase
{
    public string ResetPasswordUriTemplate { get; set; } = string.Empty;
    public string InviteUserUriTemplate { get; set; } = string.Empty;
}