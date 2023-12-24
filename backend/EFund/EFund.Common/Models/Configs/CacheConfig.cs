using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class CacheConfig : ConfigBase
{
    public TimeSpan SlidingLifetime { get; set; }
    public TimeSpan AbsoluteLifetime { get; set; }
}