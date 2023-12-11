using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class AppDataConfig : ConfigBase
{
    public string AppDataPath { get; set; } = string.Empty;
    public string LogDirectory { get; set; } = string.Empty;
}