using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(FundraisingsMockBehavior)])]
public class ReportAttachmentsMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    IRepository<FundraisingReport> fundraisingReportRepository)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly IRepository<FundraisingReport> _fundraisingReportRepository = fundraisingReportRepository;
    private readonly string[] _userEmails = ["vladd.golovatyuk@gmail.com", "ihverwork@gmail.com"];
    private readonly string[] _pictureFileNames = Enumerable.Range(1, 7).Select(i => $"{i}.jpg").ToArray();
    private readonly string[] _xlsxFileNames = Enumerable.Range(1, 2).Select(i => $"{i}.xlsx").ToArray();
    private readonly string[] _pdfFileNames = Enumerable.Range(1, 2).Select(i => $"{i}.pdf").ToArray();
    private readonly Random _random = new(217);

    protected override async Task MockData()
    {
        var reports = await _fundraisingReportRepository
            .Include(fr => fr.Fundraising)
            .ThenInclude(f => f.User)
            .Include(fr => fr.Attachments)
            .Where(fr => _userEmails.Contains(fr.Fundraising.User.Email))
            .ToListAsync();

        foreach (var report in reports)
        {
            if (report.Attachments.Count > 0)
                continue;

            var directory = AppDataConfig.GetReportAttachmentDirectoryPath(report.Id);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var attachments = GetRandomAttachments(report.Id, directory);
            foreach (var attachment in attachments)
            {
                File.Copy($"{DataFolder}Reports/{attachment.Name}", attachment.FilePath);
            }
            report.Attachments.AddRange(attachments);
            await _fundraisingReportRepository.UpdateAsync(report);
        }        
    }
    
    private List<ReportAttachment> GetRandomAttachments(Guid reportId, string directory)
    {
        var attachments = new List<ReportAttachment>();

        // Decide how many attachments to generate (1–3)
        int count = _random.Next(1, 4);

        // Shuffle file pools to ensure variation
        var shuffledPictures = _pictureFileNames.OrderBy(_ => _random.Next()).ToList();
        var shuffledPdfs = _pdfFileNames.OrderBy(_ => _random.Next()).ToList();
        var shuffledExcels = _xlsxFileNames.OrderBy(_ => _random.Next()).ToList();

        for (int i = 0; i < count; i++)
        {
            string fileName;

            // 70% chance for picture, 15% pdf, 15% xlsx
            int roll = _random.Next(100);
            if (roll < 70 && shuffledPictures.Count > 0)
            {
                fileName = shuffledPictures[0];
                shuffledPictures.RemoveAt(0);
            }
            else if (roll < 85 && shuffledPdfs.Count > 0)
            {
                fileName = shuffledPdfs[0];
                shuffledPdfs.RemoveAt(0);
            }
            else if (shuffledExcels.Count > 0)
            {
                fileName = shuffledExcels[0];
                shuffledExcels.RemoveAt(0);
            }
            else if (shuffledPictures.Count > 0) // fallback to pictures
            {
                fileName = shuffledPictures[0];
                shuffledPictures.RemoveAt(0);
            }
            else
            {
                // fallback to any available file
                fileName = _pictureFileNames.Concat(_pdfFileNames).Concat(_xlsxFileNames).OrderBy(_ => _random.Next()).First();
            }

            attachments.Add(new ReportAttachment
            {
                FundraisingReportId = reportId,
                Name = fileName,
                FilePath = Path.Combine(directory, fileName)
            });
        }

        return attachments;
    }
}