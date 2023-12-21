using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class MonobankConfig : HttpClientConfigBase
{
    public string SendAddress { get; set; } = string.Empty;
}