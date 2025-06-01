using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Complaint;
using EFund.Common.Models.DTO.Error;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Email.Models;
using EFund.Email.Services.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using EFund.Hangfire.Jobs;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class ComplaintService(
    IRepository<Complaint> complaintRepository,
    IRepository<Violation> violationRepository,
    IRepository<Fundraising> fundraisingRepository,
    IMapper mapper,
    IHangfireService hangfireService,
    IEmailSender emailSender,
    CallbackUrisConfig callbackUrisConfig)
    : IComplaintService
{
    private readonly IRepository<Complaint> _complaintRepository = complaintRepository;
    private readonly IRepository<Violation> _violationRepository = violationRepository;
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IHangfireService _hangfireService = hangfireService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly CallbackUrisConfig _callbackUrisConfig = callbackUrisConfig;

    public async Task<PagedListDTO<ComplaintDTO>> SearchAsync(SearchComplaintsDTO dto, PaginationDTO pagination)
    {
        var query = IncludeRelations(_complaintRepository);

        if (dto.Status.HasValue)
            query = query.Where(c => c.Status == dto.Status);

        if (dto.RequestedBy != Guid.Empty)
            query = query.Where(c => c.RequestedBy == dto.RequestedBy);

        if (dto.ReviewedBy.HasValue)
            query = query.Where(c => c.ReviewedBy == dto.ReviewedBy);

        var complaints = await query
            .OrderByDescending(c => c.RequestedAt)
            .ToPagedListAsync(pagination.Page, pagination.PageSize);

        return _mapper.Map<PagedListDTO<ComplaintDTO>>(complaints);
    }

    public async Task<Either<ErrorDTO, ComplaintDTO>> GetByIdAsync(Guid id)
    {
        var complaint = await IncludeRelations(_complaintRepository).FirstOrDefaultAsync(c => c.Id == id);
        if (complaint == null)
            return new NotFoundErrorDTO("Complaint not found");

        return _mapper.Map<ComplaintDTO>(complaint);
    }

    public async Task<Either<ErrorDTO, ComplaintDTO>> AddAsync(Guid requestedBy, CreateComplaintDTO dto)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == dto.FundraisingId);
        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising not found");

        var violations = dto.ViolationIds.Count > 0
            ? await _violationRepository.Where(v => dto.ViolationIds.Contains(v.Id)).ToListAsync()
            : [];

        var complaint = _mapper.Map<Complaint>(dto);
        complaint.RequestedBy = requestedBy;
        complaint.RequestedFor = fundraising.UserId;
        complaint.Violations = violations;

        await _complaintRepository.InsertAsync(complaint);

        return _mapper.Map<ComplaintDTO>(complaint);
    }

    public async Task<Option<ErrorDTO>> AcceptAsync(ComplaintAcceptDTO dto, Guid reviewedBy)
    {
        var result = await GetComplaintForReview(dto.ComplaintId);
        return await result.Match<Task<Option<ErrorDTO>>>(
            Right: async complaint =>
            {
                await SetReviewed(complaint, reviewedBy, ComplaintStatus.Accepted);

                await TriggerPostAcceptActions(dto, complaint);

                return None;
            },
            Left: error => Task.FromResult<Option<ErrorDTO>>(error));
    }

    public async Task<Option<ErrorDTO>> RejectAsync(ComplaintRejectDTO dto, Guid reviewedBy)
    {
        var result = await GetComplaintForReview(dto.ComplaintId);
        return await result.Match<Task<Option<ErrorDTO>>>(
            Right: async complaint =>
            {
                await SetReviewed(complaint, reviewedBy, ComplaintStatus.Rejected);
                return None;
            },
            Left: error => Task.FromResult<Option<ErrorDTO>>(error));
    }

    public async Task<Option<ErrorDTO>> RequestChangesAsync(ComplaintRequestChangesDTO dto, Guid reviewedBy)
    {
        var result = await GetComplaintForReview(dto.ComplaintId);
        return await result.Match<Task<Option<ErrorDTO>>>(
            Right: async complaint =>
            {
                await SetReviewed(complaint, reviewedBy, ComplaintStatus.RequestedChanges);

                await TriggerPostRequestChangesActions(dto, complaint);

                return None;
            },
            Left: error => Task.FromResult<Option<ErrorDTO>>(error));
    }

    public async Task<ComplaintTotalsDTO> GetTotalsAsync()
    {
        var statuses = Enum.GetValues<ComplaintStatus>();
        var totals = await _complaintRepository
            .GroupBy(c => c.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            }).ToListAsync();

        return new ComplaintTotalsDTO
        {
            TotalsByStatus = statuses.ToDictionary(
                status => status,
                status => totals.FirstOrDefault(t => t.Status == status)?.Count ?? 0)
        };
    }

    private async Task<Either<ErrorDTO, Complaint>> GetComplaintForReview(Guid complaintId)
    {
        var complaint = await _complaintRepository
            .Include(c => c.Fundraising)
            .Include(c => c.Violations)
            .Include(c => c.RequestedForUser)
            .Include(c => c.ReviewedByUser)
            .FirstOrDefaultAsync(c => c.Id == complaintId);

        if (complaint == null)
            return new NotFoundErrorDTO("Complaint not found");

        if (complaint.Status != ComplaintStatus.Pending)
            return new IncorrectParametersErrorDTO("Complaint is already reviewed");

        return complaint;
    }

    private async Task SetReviewed(Complaint complaint, Guid reviewedBy, ComplaintStatus status)
    {
        complaint.Status = status;
        complaint.ReviewedAt = DateTimeOffset.UtcNow;
        complaint.ReviewedBy = reviewedBy;

        await _complaintRepository.Where(c => c.Id == complaint.Id).ExecuteUpdateAsync(s => s
            .SetProperty(c => c.Status, status)
            .SetProperty(c => c.ReviewedAt, DateTimeOffset.UtcNow)
            .SetProperty(c => c.ReviewedBy, reviewedBy));
    }

    private async Task TriggerPostAcceptActions(ComplaintAcceptDTO dto, Complaint complaint)
    {
        var callbackUri = string.Format(_callbackUrisConfig.ViewFundraisingUriTemplate, complaint.FundraisingId);
        var violations = string.Join(", ", complaint.Violations.Select(v => $"\"{v.Title}\"").ToList());
        await _emailSender.SendEmailAsync(complaint.RequestedForUser.Email!, new ContentRemovalMessage
        {
            UserName = complaint.RequestedForUser.DisplayName,
            ViewFundraisingUri = callbackUri,
            Violations = violations
        });

        _hangfireService.Enqueue<ChangeFundraisingStatusJob, ChangeFundraisingStatusJobArgs>(
            new ChangeFundraisingStatusJobArgs
            {
                FundraisingId = complaint.FundraisingId,
                FundraisingStatus = FundraisingStatus.Deleted
            });

        _hangfireService.Enqueue<UpdateUserRatingJob, UpdateUserRatingJobArgs>(
            new UpdateUserRatingJobArgs
            {
                UserId = complaint.RequestedFor,
                RatingChange = dto.RatingChange
            });

        _hangfireService.Enqueue<SaveComplaintResponseNotificationForRequestedByJob, SaveComplaintRepsonseNotificationForRequestedByJobArgs>(
            new SaveComplaintRepsonseNotificationForRequestedByJobArgs
            {
                UserId = complaint.RequestedBy
            });

        _hangfireService.Enqueue<SaveAcceptedComplaintNotificationForRequestedForJob, SaveAcceptedComplaintNotificationForRequestedForJobArgs>(
            new SaveAcceptedComplaintNotificationForRequestedForJobArgs
            {
                UserId = complaint.RequestedFor,
                FundraisingTitle = complaint.Fundraising.Title,
                Violations = complaint.Violations.Select(v => v.Title).ToList()
            });
    }

    private async Task TriggerPostRequestChangesActions(ComplaintRequestChangesDTO dto, Complaint complaint)
    {
        var callbackUri = string.Format(_callbackUrisConfig.ViewFundraisingUriTemplate, complaint.FundraisingId);
        await _emailSender.SendEmailAsync(complaint.RequestedForUser.Email!, new ContentVisibilityMessage
        {
            UserName = complaint.RequestedForUser.DisplayName,
            ViewFundraisingUri = callbackUri,
            Message = dto.Message
        });

        _hangfireService.Enqueue<ChangeFundraisingStatusJob, ChangeFundraisingStatusJobArgs>(
            new ChangeFundraisingStatusJobArgs
            {
                FundraisingId = complaint.FundraisingId,
                FundraisingStatus = FundraisingStatus.Hidden
            });

        _hangfireService.Enqueue<SaveRequestedChangesComplaintNotificationForRequestedForJob, SaveRequestedChangesComplaintNotificationForRequestedForJobArgs>(
            new SaveRequestedChangesComplaintNotificationForRequestedForJobArgs
            {
                UserId = complaint.RequestedFor,
                Message = dto.Message,
                FundraisingTitle = complaint.Fundraising.Title
            });
        
        _hangfireService.Enqueue<SaveComplaintResponseNotificationForRequestedByJob, SaveComplaintRepsonseNotificationForRequestedByJobArgs>(
            new SaveComplaintRepsonseNotificationForRequestedByJobArgs
            {
                UserId = complaint.RequestedBy
            });
    }

    private IQueryable<Complaint> IncludeRelations(IQueryable<Complaint> query)
    {
        return query
            .Include(c => c.Violations)
            .Include(c => c.RequestedByUser)
            .Include(c => c.RequestedForUser)
            .Include(c => c.ReviewedByUser);
    }
}