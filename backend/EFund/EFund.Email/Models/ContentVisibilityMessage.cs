using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class ContentVisibilityMessage : EmailMessageBase
{
    public override string Subject => "EFund Content Removal Notice";
    public override string TemplateName => nameof(ContentVisibilityMessage);
    public string UserName { get; set; } = string.Empty;
    public string ViewFundraisingUri { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}