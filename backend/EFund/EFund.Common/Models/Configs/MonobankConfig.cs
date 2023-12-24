using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class MonobankConfig : HttpClientConfigBase
{
    public string SendAddress { get; set; } = string.Empty;
    public TimeSpan ClientInfoCacheSlidingLifetime { get; set; }
    public TimeSpan ClientInfoCacheAbsoluteLifetime { get; set; }
    public TimeSpan ClientInfoCacheBackupSlidingLifetime { get; set; }
    public TimeSpan ClientInfoCacheBackupAbsoluteLifetime { get; set; }
}