using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.BLL.Utility;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Common;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Fundraising;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class FundraisingService : IFundraisingService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Fundraising> _fundraisingRepository;
    private readonly IRepository<MonobankFundraising> _monobankFundraisings;
    private readonly IMonobankService _monobankService;
    private readonly AppDataConfig _appDataConfig;
    private readonly IRepository<Tag> _tagRepository;

    public FundraisingService(IMapper mapper,
        IRepository<Fundraising> fundraisingRepository,
        IMonobankService monobankService,
        AppDataConfig appDataConfig,
        IRepository<Tag> tagRepository,
        IRepository<MonobankFundraising> monobankFundraisings)
    {
        _mapper = mapper;
        _fundraisingRepository = fundraisingRepository;
        _monobankService = monobankService;
        _appDataConfig = appDataConfig;
        _tagRepository = tagRepository;
        _monobankFundraisings = monobankFundraisings;
    }

    public async Task<Either<ErrorDTO, PagedListDTO<FundraisingDTO>>> GetAllAsync(Guid userId, PaginationDTO pagination,
        string apiUrl)
    {
        var fundraisings = await IncludeRelations(_fundraisingRepository)
            .Where(f => f.UserId == userId)
            .ToPagedListAsync(pagination.Page, pagination.PageSize);

        return await ToDto(fundraisings, apiUrl);
    }

    public async Task<Either<ErrorDTO, PagedListDTO<FundraisingDTO>>> Search(SearchFundraisingDTO dto,
        PaginationDTO pagination,
        string apiUrl)
    {
        IQueryable<Fundraising> queryable = IncludeRelations(_fundraisingRepository);

        if (dto.Statuses.Count > 0)
            queryable = queryable.Where(f => dto.Statuses.Contains(f.Status));

        if (!string.IsNullOrEmpty(dto.Title))
            queryable = queryable.Where(f => f.Title.Contains(dto.Title));

        if (dto.Tags.Count > 0)
            queryable = queryable.Where(f => f.Tags.Any(t => dto.Tags.Contains(t.Name)));

        if (dto.UserId != null)
            queryable = queryable.Where(f => f.UserId == dto.UserId);

        var fundraisings = await queryable.ToPagedListAsync(pagination.Page, pagination.PageSize);
        return await ToDto(fundraisings, apiUrl);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> GetByIdAsync(Guid id, string apiUrl)
    {
        var fundraising = await IncludeRelations(_fundraisingRepository)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        return await ToDto(fundraising, apiUrl);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> AddAsync(Guid userId, CreateFundraisingDTO dto, string apiUrl)
    {
        var fundraising = _mapper.Map<Fundraising>(dto);
        fundraising.UserId = userId;

        fundraising.Tags = await GetTags(dto.Tags);

        await _monobankFundraisings.InsertAsync(fundraising.MonobankFundraising, persist: false);
        await _fundraisingRepository.InsertAsync(fundraising);
        return await ToDto(fundraising, apiUrl);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> UpdateAsync(Guid id, Guid userId, UpdateFundraisingDTO dto,
        string apiUrl)
    {
        var fundraising = await IncludeRelations(_fundraisingRepository)
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        _mapper.Map(dto, fundraising);
        fundraising.Tags = await GetTags(dto.Tags);

        await _fundraisingRepository.UpdateAsync(fundraising);

        return await ToDto(fundraising, apiUrl);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> UpdateStatusAsync(Guid id, Guid userId,
        UpdateFundraisingStatusDTO dto, string apiUrl)
    {
        var fundraising = await IncludeRelations(_fundraisingRepository)
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        _mapper.Map(dto, fundraising);
        await _fundraisingRepository.UpdateAsync(fundraising);

        return await ToDto(fundraising, apiUrl);
    }

    public async Task<Option<ErrorDTO>> DeleteAsync(Guid id)
    {
        var fundraising = await _fundraisingRepository
            .Include(f => f.Reports)
            .ThenInclude(r => r.Attachments)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        if (fundraising.AvatarPath != null && File.Exists(fundraising.AvatarPath))
            File.Delete(fundraising.AvatarPath);

        foreach (var attachment in fundraising.Reports.SelectMany(r => r.Attachments)
                     .Where(a => File.Exists(a.FilePath)))
        {
            File.Delete(attachment.FilePath);
        }

        await _fundraisingRepository.DeleteAsync(fundraising);

        return None;
    }

    public async Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId)
    {
        var fundraising = await _fundraisingRepository
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        fundraising.Status = FundraisingStatus.Deleted;

        await _fundraisingRepository.UpdateAsync(fundraising);

        return None;
    }

    public async Task<Option<ErrorDTO>> UploadAvatarAsync(Guid id, Guid userId, IFormFile file)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        if (fundraising is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (!_appDataConfig.AllowedImages.ContainsKey(file.ContentType))
            return new IncorrectParametersErrorDTO("This type of files is not allowed");

        if (fundraising.AvatarPath != null && File.Exists(fundraising.AvatarPath))
            File.Delete(fundraising.AvatarPath);

        var directory = Path.Combine(_appDataConfig.UserAvatarDirectoryPath, fundraising.Id.ToString());

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        fundraising.AvatarPath = Path.Combine(directory,
            $"{_appDataConfig.AvatarFileName}{Guid.NewGuid():N}{_appDataConfig.AllowedImages[file.ContentType]}");

        await using var outputStream = File.Create(fundraising.AvatarPath);
        await using var inputStream = file.OpenReadStream();
        await inputStream.CopyToAsync(outputStream);

        await _fundraisingRepository.UpdateAsync(fundraising);

        return None;
    }

    public async Task<Option<ErrorDTO>> DeleteAvatarAsync(Guid id, Guid userId)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        if (fundraising is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (fundraising.AvatarPath is null)
            return None;

        if (File.Exists(fundraising.AvatarPath))
            File.Delete(fundraising.AvatarPath);

        fundraising.AvatarPath = null;
        await _fundraisingRepository.UpdateAsync(fundraising);

        return None;
    }

    private void ConvertFundraisingPathToUrl(Fundraising fundraising, string apiUrl)
    {
        fundraising.AvatarPath = (fundraising.AvatarPath ?? _appDataConfig.DefaultFundraisingAvatarPath).PathToUrl(apiUrl);

        foreach (var report in fundraising.Reports)
        {
            report.Attachments.ForEach(a => a.FilePath = a.FilePath.PathToUrl(apiUrl));
        }
    }

    private string GetUserAvatarUrl(Fundraising fundraising, string apiUrl)
    {
        return (fundraising.User?.AvatarPath ?? _appDataConfig.DefaultUserAvatarPath).PathToUrl(apiUrl);
    }

    private async Task<Either<ErrorDTO, FundraisingDTO>> ToDto(Fundraising fundraising, string apiUrl)
    {
        ConvertFundraisingPathToUrl(fundraising, apiUrl);
        var result = await _monobankService.GetJarByIdAsync(fundraising.UserId, fundraising.MonobankFundraising.JarId);
        return result.Match<Either<ErrorDTO, FundraisingDTO>>(
            Right: jar =>
            {
                var fundraisingDto = _mapper.Map<FundraisingDTO>(fundraising);
                fundraisingDto.MonobankJar = jar;
                fundraisingDto.UserAvatarUrl = GetUserAvatarUrl(fundraising, apiUrl);
                return fundraisingDto;
            },
            Left: error => error
        );
    }

    private async Task<Either<ErrorDTO, PagedListDTO<FundraisingDTO>>> ToDto(PagedList<Fundraising> fundraisings,
        string apiUrl)
    {
        fundraisings.ForEach(f => ConvertFundraisingPathToUrl(f, apiUrl));

        var dtos = _mapper.Map<PagedListDTO<FundraisingDTO>>(fundraisings);

        var userIds = dtos.Items.Select(f => f.UserId).Distinct().ToList();

        var result = await _monobankService.GetJarsAsync(userIds);
        return result.Match<Either<ErrorDTO, PagedListDTO<FundraisingDTO>>>(
            Right: jars =>
            {
                foreach (var (fundraising, dto) in fundraisings
                             .Join(dtos.Items, f => f.Id, dto => dto.Id, (f, dto) => (f, dto)))
                {
                    dto.UserAvatarUrl = GetUserAvatarUrl(fundraising, apiUrl);
                    dto.MonobankJar = jars.FirstOrDefault(j => j.Id == dto.MonobankJarId);
                }
                return dtos;
            },
            Left: error => error
        );
    }

    private IQueryable<Fundraising> IncludeRelations(IQueryable<Fundraising> queryable)
    {
        return queryable
            .Include(f => f.User)
            .Include(f => f.Tags)
            .Include(f => f.MonobankFundraising)
            .Include(f => f.Reports)
            .ThenInclude(r => r.Attachments);
    }

    private async Task<List<Tag>> GetTags(List<string> tags)
    {
        tags = tags.Select(t => t.ToLower()).ToList();
        var existingTags = await _tagRepository.Where(t => tags.Contains(t.Name)).ToListAsync();
        var newTags = tags
            .Where(t => existingTags.All(et => et.Name != t))
            .Select(t => new Tag { Name = t })
            .ToList();

        await _tagRepository.InsertManyAsync(newTags);

        return existingTags.Union(newTags).ToList();
    }
}