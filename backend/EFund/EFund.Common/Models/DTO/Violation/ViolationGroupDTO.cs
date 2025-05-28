namespace EFund.Common.Models.DTO.Violation;

public class ViolationGroupDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<ViolationDTO> Violations { get; set; } = [];
}