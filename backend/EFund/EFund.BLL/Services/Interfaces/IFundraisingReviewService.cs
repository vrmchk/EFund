using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReview;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IFundraisingReviewService
{
    Task<Either<ErrorDTO, FundraisingReviewDTO>> AddReviewAsync(Guid reviewedBy, CreateFundraisingReviewDTO dto);
    Task<Either<ErrorDTO, List<FundraisingReviewDTO>>> GetAllAsync(Guid? fundraisingId, Guid? reviewId);
    Task<Either<ErrorDTO, FundraisingReviewDTO>> GetByIdAsync(Guid id);
    Task<Either<ErrorDTO, FundraisingReviewDTO>> UpdateAsync(Guid id, UpdateFundraisingReviewDTO dto);
}