using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class EmailConfirmationMessage : EmailMessageBase
{
    public override string Subject => "Email Confirmation";
    public override string TemplateName => nameof(EmailConfirmationMessage);
    public int Code { get; set; }
}