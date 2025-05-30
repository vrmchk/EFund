using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Common.Models.DTO.ReportAttachment;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class FundraisingReportService : IFundraisingReportService
{
    private readonly IMapper _mapper;
    private readonly IRepository<FundraisingReport> _reportRepository;
    private readonly IRepository<Fundraising> _fundraisingRepository;
    private readonly IRepository<ReportAttachment> _attachmentRepository;
    private readonly AppDataConfig _appDataConfig;

    public FundraisingReportService(IMapper mapper,
        IRepository<FundraisingReport> reportRepository,
        IRepository<Fundraising> fundraisingRepository,
        IRepository<ReportAttachment> attachmentRepository,
        AppDataConfig appDataConfig)
    {
        _mapper = mapper;
        _reportRepository = reportRepository;
        _fundraisingRepository = fundraisingRepository;
        _attachmentRepository = attachmentRepository;
        _appDataConfig = appDataConfig;
    }

    public async Task<Either<ErrorDTO, FundraisingReportDTO>> GetByIdAsync(Guid id, Guid userId, string apiUrl)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        return ToDto(report, apiUrl);
    }

    public async Task<Either<ErrorDTO, FundraisingReportDTO>> AddAsync(Guid userId, CreateFundraisingReportDTO dto)
    {
        var fundraising = await _fundraisingRepository
            .FirstOrDefaultAsync(f => f.Id == dto.FundraisingId && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        var report = _mapper.Map<FundraisingReport>(dto);

        await _reportRepository.InsertAsync(report);

        return _mapper.Map<FundraisingReportDTO>(report);
    }

    public async Task<Either<ErrorDTO, FundraisingReportDTO>> UpdateAsync(Guid id, Guid userId,
        UpdateFundraisingReportDTO dto, string apiUrl)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        _mapper.Map(dto, report);

        await _reportRepository.UpdateAsync(report);

        return ToDto(report, apiUrl);
    }

    public async Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        foreach (var attachment in report.Attachments.Where(a => File.Exists(a.FilePath)))
        {
            File.Delete(attachment.FilePath);
        }

        await _reportRepository.DeleteAsync(report);
        return None;
    }

    public async Task<Option<ErrorDTO>> AddAttachmentsAsync(Guid reportId, Guid userId, IFormFileCollection files)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == reportId && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        if (files.Any(f => !_appDataConfig.AllowedFiles.ContainsKey(f.ContentType)))
            return new IncorrectParametersErrorDTO("Some files have invalid format");

        var directory = _appDataConfig.GetReportAttachmentDirectoryPath(reportId);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var attachments = new Dictionary<ReportAttachment, IFormFile>();
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file.FileName);
            if (attachments.Any(a => a.Key.Name == name)
                || report.Attachments.Any(a => a.Name == name))
                name += "(1)";

            var attachment = new ReportAttachment
            {
                FundraisingReportId = reportId,
                Name = name,
                FilePath = _appDataConfig.GetReportAttachmentFilePath(directory, name, file.ContentType)
            };

            attachments.Add(attachment, file);
        }

        await Task.WhenAll(attachments.Select(async pair =>
        {
            await using var outputStream = File.Create(pair.Key.FilePath);
            await using var inputStream = pair.Value.OpenReadStream();
            await inputStream.CopyToAsync(outputStream);
        }));

        await _attachmentRepository.InsertManyAsync(attachments.Keys);

        return None;
    }

    public async Task<Option<ErrorDTO>> UpdateAttachmentAsync(Guid reportId, Guid attachmentId, Guid userId,
        UpdateAttachmentDTO dto)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == reportId && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        var attachment = report.Attachments.SingleOrDefault(a => a.Id == attachmentId);
        if (attachment == null)
            return new NotFoundErrorDTO("Attachment with this id does not exist");

        _mapper.Map(dto, attachment);
        await _attachmentRepository.UpdateAsync(attachment);

        return None;
    }

    public async Task<Option<ErrorDTO>> DeleteAttachmentsAsync(Guid reportId, Guid userId, Guid attachmentId)
    {
        var report = await _reportRepository
            .Include(r => r.Fundraising)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == reportId && r.Fundraising.UserId == userId);

        if (report == null)
            return new NotFoundErrorDTO("Report with this id does not exist");

        var attachment = report.Attachments.SingleOrDefault(a => a.Id == attachmentId);
        if (attachment == null)
            return new NotFoundErrorDTO("Attachment with this id does not exist");

        if (File.Exists(attachment.FilePath))
            File.Delete(attachment.FilePath);

        await _attachmentRepository.DeleteAsync(attachment);

        return None;
    }

    private FundraisingReportDTO ToDto(FundraisingReport report, string apiUrl)
    {
        foreach (var attachment in report.Attachments)
        {
            attachment.FilePath = attachment.FilePath.PathToUrl(apiUrl);
        }

        var dto = _mapper.Map<FundraisingReportDTO>(report);
        return dto;
    }
}