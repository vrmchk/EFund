namespace EFund.Common.Models.DTO.User;

public class UserExtendedDTO : UserDTO
{
    public bool EmailConfirmed { get; set; }
    public bool CreatedByAdmin { get; set; }
    public bool IsBlocked { get; set; }
}