namespace EFund.Common.Models.DTO.FundraisingReview;

public class CreateFundraisingReviewDTO
{
    public Guid FundraisingId { get; set; }
    public decimal RatingChange { get; set; }
    public string Comment { get; set; } = string.Empty;
}