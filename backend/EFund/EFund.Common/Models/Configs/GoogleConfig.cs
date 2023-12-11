using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class GoogleConfig : ConfigBase
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string RedirectUri { get; set; } = string.Empty;

    public TimeSpan ClockTolerance { get; set; }
}