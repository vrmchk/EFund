namespace EFund.Common.Models.DTO.Violation;

public class ViolationDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public decimal RatingImpact { get; set; }
    public Guid ViolationGroupId { get; set; }
}