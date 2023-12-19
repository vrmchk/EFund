using AutoMapper;
using EFund.BLL.Extensions;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO;
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
    private readonly IMonobankService _monobankService;
    private readonly AppDataConfig _appDataConfig;

    public FundraisingService(IMapper mapper,
        IRepository<Fundraising> fundraisingRepository,
        IMonobankService monobankService,
        AppDataConfig appDataConfig)
    {
        _mapper = mapper;
        _fundraisingRepository = fundraisingRepository;
        _monobankService = monobankService;
        _appDataConfig = appDataConfig;
    }

    public async Task<Either<ErrorDTO, List<FundraisingDTO>>> Search(SearchFundraisingDTO dto, PaginationDTO pagination,
        string apiUrl)
    {
        IQueryable<Fundraising> queryable = IncludeRelations(_fundraisingRepository);

        if (!dto.IncludeClosed)
            queryable = queryable.Where(f => !f.IsClosed);

        if (!string.IsNullOrEmpty(dto.Title))
            queryable = queryable.Where(f => f.Title.Contains(dto.Title));

        if (dto.Tags.Count > 0)
            queryable = queryable.Where(f => f.Tags.Any(t => dto.Tags.Contains(t.Name)));

        var fundraisings = await queryable.ToPagedListAsync(pagination.PageNumber, pagination.PageSize);
        foreach (var fundraising in fundraisings)
        {
            fundraising.AvatarPath = (fundraising.AvatarPath ?? _appDataConfig.DefaultFundraisingAvatarPath)
                .PathToUrl(apiUrl);
        }

        var dtos = _mapper.Map<List<FundraisingDTO>>(fundraisings);

        var userIds = dtos.Select(f => f.UserId).Distinct().ToList();

        var result = await _monobankService.GetJarsAsync(userIds);
        return result.Match<Either<ErrorDTO, List<FundraisingDTO>>>(
            Right: jars =>
            {
                dtos.ForEach(f => f.MonobankJar = jars.FirstOrDefault(j => j.Id == f.MonobankJarId));
                return dtos;
            },
            Left: error => error
        );
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> GetByIdAsync(Guid id, string apiUrl)
    {
        var fundraising = await IncludeRelations(_fundraisingRepository)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        fundraising.AvatarPath = (fundraising.AvatarPath ?? _appDataConfig.DefaultFundraisingAvatarPath)
            .PathToUrl(apiUrl);

        return await ToDto(fundraising);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> AddAsync(Guid userId, CreateFundraisingDTO dto)
    {
        var fundraising = _mapper.Map<Fundraising>(dto);
        fundraising.UserId = userId;

        await _fundraisingRepository.InsertAsync(fundraising);
        return await ToDto(fundraising);
    }

    public async Task<Either<ErrorDTO, FundraisingDTO>> UpdateAsync(Guid id, Guid userId, UpdateFundraisingDTO dto,
        string apiUrl)
    {
        var fundraising = await IncludeRelations(_fundraisingRepository)
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        _mapper.Map(dto, fundraising);

        await _fundraisingRepository.UpdateAsync(fundraising);

        fundraising.AvatarPath = (fundraising.AvatarPath ?? _appDataConfig.DefaultFundraisingAvatarPath)
            .PathToUrl(apiUrl);

        return await ToDto(fundraising);
    }

    public async Task<Option<ErrorDTO>> DeleteAsync(Guid id)
    {
        var fundraising = await _fundraisingRepository
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        await _fundraisingRepository.DeleteAsync(fundraising);

        return None;
    }    

    public async Task<Option<ErrorDTO>> DeleteAsync(Guid id, Guid userId)
    {
        var fundraising = await _fundraisingRepository
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

        if (fundraising == null)
            return new NotFoundErrorDTO("Fundraising with this id does not exist");

        await _fundraisingRepository.DeleteAsync(fundraising);

        return None;
    }

    public async Task<Option<ErrorDTO>> UploadAvatarAsync(Guid id, Guid userId, IFormFile file)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        if (fundraising is null)
            return new NotFoundErrorDTO("User with this id does not exist");

        if (!_appDataConfig.AllowedImageFileTypes.Contains(file.ContentType))
            return new IncorrectParametersErrorDTO("This type of files is not allowed");

        if (fundraising.AvatarPath != null)
            File.Delete(fundraising.AvatarPath);

        var directory = Path.Combine(_appDataConfig.UserAvatarDirectoryPath, fundraising.Id.ToString());

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        fundraising.AvatarPath = Path.Combine(directory,
            $"{_appDataConfig.AvatarFileName}{file.ContentType.MimeTypeToFileExtension()}");

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

        File.Delete(fundraising.AvatarPath);
        fundraising.AvatarPath = null;
        await _fundraisingRepository.UpdateAsync(fundraising);

        return None;
    }

    private async Task<Either<ErrorDTO, FundraisingDTO>> ToDto(Fundraising fundraising)
    {
        var result = await _monobankService.GetJarByIdAsync(fundraising.UserId, fundraising.MonobankFundraising.JarId);
        return result.Match<Either<ErrorDTO, FundraisingDTO>>(
            Right: jar =>
            {
                var fundraisingDto = _mapper.Map<FundraisingDTO>(fundraising);
                fundraisingDto.MonobankJar = jar;
                return fundraisingDto;
            },
            Left: error => error
        );
    }

    private IQueryable<Fundraising> IncludeRelations(IQueryable<Fundraising> queryable)
    {
        return queryable
            .Include(f => f.Tags)
            .Include(f => f.MonobankFundraising)
            .Include(f => f.Reports)
            .ThenInclude(r => r.Attachments);
    }
}