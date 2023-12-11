using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class PasswordChangedMessage : EmailMessageBase
{
    public override string Subject => "Security Notification";
    public override string TemplateName => nameof(PasswordChangedMessage);
    public string UserName { get; set; } = string.Empty;
}