using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class ContentRemovalMessage : EmailMessageBase
{
    public override string Subject => "EFund Fundraising Visibility Notice";
    public override string TemplateName => nameof(ContentRemovalMessage);
    public string UserName { get; set; } = string.Empty;
    public string ViewFundraisingUri { get; set; } = string.Empty;
    public string Violations { get; set; } = string.Empty;
}