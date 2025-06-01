using EFund.Common.Models.Configs.Abstract;

namespace EFund.Common.Models.Configs;

public class EmailConfig : ConfigBase
{
    public string SmtpServer { get; set; } = string.Empty;
    public string DefaultEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TemplatesPath { get; set; } = string.Empty;
    public string SmtpPortString { get; set; } = string.Empty;
    public int SmtpPort => int.Parse(SmtpPortString);
}