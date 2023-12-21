namespace EFund.Common.Models.DTO.User;

public class SearchUserDTO
{
    public List<Guid>? UserIds { get; set; }
    public List<string>? Emails { get; set; }
    public List<string>? UserNames { get; set; }
    public bool? CreatedByAdmin { get; set; }
    public bool? IsBlocked { get; set; }
    public bool? EmailConfirmed { get; set; }
}