namespace EFund.Common.Models.Configs.Abstract;

public class HttpClientConfigBase : ConfigBase
{
    public string HttpClientName { get; set; } = string.Empty;
    public string BaseAddress { get; set; } = string.Empty;
}