using EFund.Email.Models.Base;

namespace EFund.Email.Models;

public class ResetPasswordMessage : EmailMessageBase
{
    public override string Subject => "Reset Password";
    public override string TemplateName => nameof(ResetPasswordMessage);
    public string UserName { get; set; } = string.Empty;
    public string ResetPasswordUri { get; set; } = string.Empty;
}