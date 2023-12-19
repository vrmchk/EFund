using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class AdminInvitationMessage : EmailMessageBase
{
    public override string Subject => "EFund - Admin Invitation";
    public override string TemplateName => nameof(AdminInvitationMessage);
    public string InvitationUri { get; set; } = string.Empty;
}