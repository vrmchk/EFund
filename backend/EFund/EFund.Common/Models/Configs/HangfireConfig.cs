using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class HangfireConfig : ConfigBase
{
    public string ClearExpiredUserRegistrationsCron { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
