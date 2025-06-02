using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Fundraising;

public class UpdateFundraisingStatusByAdminDTO
{
    public FundraisingStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;
}