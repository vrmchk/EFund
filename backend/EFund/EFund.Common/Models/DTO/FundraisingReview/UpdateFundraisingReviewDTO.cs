namespace EFund.Common.Models.DTO.FundraisingReview;

public class UpdateFundraisingReviewDTO
{
    public decimal RatingChange { get; set; }
    public string Comment { get; set; } = string.Empty;
}