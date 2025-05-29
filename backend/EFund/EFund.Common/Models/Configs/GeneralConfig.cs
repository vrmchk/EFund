using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class GeneralConfig : ConfigBase
{
    public decimal UserRatingLimit { get; set; }
}