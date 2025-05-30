using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReview;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class FundraisingReviewService(
    IRepository<Fundraising> fundraisingRepository,
    IRepository<FundraisingReview> reviewRepository,
    IMapper mapper)
    : IFundraisingReviewService
{
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly IRepository<FundraisingReview> _reviewRepository = reviewRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Either<ErrorDTO, FundraisingReviewDTO>> AddReviewAsync(Guid reviewedBy,
        CreateFundraisingReviewDTO dto)
    {
        var fundraising = await _fundraisingRepository
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.Id == dto.FundraisingId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        if (fundraising.UserId == reviewedBy)
            return new IncorrectParametersErrorDTO("You cannot review your own fundraising");

        if (fundraising.Status != FundraisingStatus.ReadyForReview)
            return new IncorrectParametersErrorDTO("You can only ready for review fundraisings");

        fundraising.Status = FundraisingStatus.Reviewed;
        fundraising.ReviewedAt = DateTimeOffset.UtcNow;
        fundraising.User.Rating += dto.RatingChange;
        await _fundraisingRepository.UpdateAsync(fundraising);

        var review = _mapper.Map<FundraisingReview>(dto);
        review.ReviewedBy = reviewedBy;

        await _reviewRepository.InsertAsync(review);

        return _mapper.Map<FundraisingReviewDTO>(review);
    }

    public async Task<Either<ErrorDTO, List<FundraisingReviewDTO>>> GetAllAsync(Guid? fundraisingId, Guid? reviewId)
    {
        if (fundraisingId == null && reviewId == null)
            return new IncorrectParametersErrorDTO("At least one of the parameters must be provided");

        var queryable = _reviewRepository.AsNoTracking();

        if (reviewId != null)
            queryable = queryable.Where(r => r.Id == reviewId);

        if (fundraisingId != null)
            queryable = queryable.Where(r => r.FundraisingId == fundraisingId);

        return _mapper.Map<List<FundraisingReviewDTO>>(await queryable.ToListAsync());
    }

    public async Task<Either<ErrorDTO, FundraisingReviewDTO>> GetByIdAsync(Guid id)
    {
        var review = await _reviewRepository.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
            return new NotFoundErrorDTO("Review with this id does not exist");

        return _mapper.Map<FundraisingReviewDTO>(review);
    }
    
    public async Task<Either<ErrorDTO, FundraisingReviewDTO>> UpdateAsync(Guid id, UpdateFundraisingReviewDTO dto)
    {
        var review = await _reviewRepository
            .Include(r => r.Fundraising)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
            return new NotFoundErrorDTO("Review with this id does not exist");

        review.Fundraising.User.Rating -= review.RatingChange; // Remove old rating change
        review.Fundraising.User.Rating += dto.RatingChange; // Apply new rating change

        _mapper.Map(dto, review);

        await _reviewRepository.UpdateAsync(review);

        return _mapper.Map<FundraisingReviewDTO>(review);
    }
}