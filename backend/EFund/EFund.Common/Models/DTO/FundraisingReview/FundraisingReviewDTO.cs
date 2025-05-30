namespace EFund.Common.Models.DTO.FundraisingReview;

public class FundraisingReviewDTO
{
    public Guid Id { get; set; }
    public Guid ReviewedBy { get; set; }
    public Guid FundraisingId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal RatingChange { get; set; }
    public string Comment { get; set; } = string.Empty;
}