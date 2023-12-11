namespace EFund.Common.Models.DTO.User;

public class ConfirmChangeEmailDTO
{
    public string NewEmail { get; set; } = string.Empty;
    public int Code { get; set; }
}