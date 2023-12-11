using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class EncryptionConfig : ConfigBase
{
    public string Key { get; set; } = string.Empty;
}
