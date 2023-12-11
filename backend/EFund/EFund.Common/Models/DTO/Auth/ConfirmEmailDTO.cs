namespace EFund.Common.Models.DTO.Auth;

public class ConfirmEmailDTO
{
    public Guid UserId { get; set; }
    public int Code { get; set; }
}