using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReview;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using EFund.Hangfire.Jobs;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class FundraisingReviewService(
    IRepository<Fundraising> fundraisingRepository,
    IRepository<FundraisingReview> reviewRepository,
    IMapper mapper,
    IHangfireService hangfireService)
    : IFundraisingReviewService
{
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly IRepository<FundraisingReview> _reviewRepository = reviewRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IHangfireService _hangfireService = hangfireService;

    public async Task<Either<ErrorDTO, FundraisingReviewDTO>> AddReviewAsync(Guid reviewedBy,
        CreateFundraisingReviewDTO dto)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == dto.FundraisingId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        if (fundraising.UserId == reviewedBy)
            return new IncorrectParametersErrorDTO("You cannot review your own fundraising");

        if (fundraising.Status != FundraisingStatus.ReadyForReview)
            return new IncorrectParametersErrorDTO("You can only ready for review fundraisings");

        fundraising.Status = FundraisingStatus.Reviewed;
        fundraising.ReviewedAt = DateTimeOffset.UtcNow;
        await _fundraisingRepository.UpdateAsync(fundraising);

        var review = _mapper.Map<FundraisingReview>(dto);
        review.ReviewedBy = reviewedBy;

        await _reviewRepository.InsertAsync(review);

        UpdateUserRating(fundraising.UserId, dto.RatingChange);

        return _mapper.Map<FundraisingReviewDTO>(review);
    }

    private void UpdateUserRating(Guid userId, decimal ratingChange)
    {
        _hangfireService.Enqueue<UpdateUserRatingJob, UpdateUserRatingJobArgs>(new UpdateUserRatingJobArgs
        {
            UserId = userId,
            RatingChange = ratingChange
        });
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
        var review = await _reviewRepository.Include(r => r.Fundraising).FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
            return new NotFoundErrorDTO("Review with this id does not exist");

        UpdateUserRating(review.Fundraising.UserId, -review.RatingChange);
        UpdateUserRating(review.Fundraising.UserId, dto.RatingChange);

        _mapper.Map(dto, review);

        await _reviewRepository.UpdateAsync(review);

        return _mapper.Map<FundraisingReviewDTO>(review);
    }
}