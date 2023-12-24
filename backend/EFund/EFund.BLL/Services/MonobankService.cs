using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Client.Monobank;
using EFund.Client.Monobank.Models.Requests;
using EFund.Common.Models.Configs;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Monobank;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace EFund.BLL.Services;

public class MonobankService : IMonobankService
{
    private readonly IMonobankClient _monobankClient;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<UserMonobank> _monobankRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IMapper _mapper;
    private readonly MonobankConfig _monobankConfig;

    public MonobankService(
        IMonobankClient monobankClient,
        UserManager<User> userManager,
        IRepository<UserMonobank> monobankRepository,
        IEncryptionService encryptionService,
        IMapper mapper,
        MonobankConfig monobankConfig)
    {
        _monobankClient = monobankClient;
        _userManager = userManager;
        _monobankRepository = monobankRepository;
        _encryptionService = encryptionService;
        _mapper = mapper;
        _monobankConfig = monobankConfig;
    }

    public async Task<Option<ErrorDTO>> AddOrUpdateMonobankTokenAsync(Guid userId, string monobankToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return new NotFoundErrorDTO("User with this id does not exist");

        var clientInfoResult = await _monobankClient.GetClientInfoAsync(new ClientInfoRequest(monobankToken));
        if (clientInfoResult.IsLeft)
            return new IncorrectParametersErrorDTO("Invalid token passed.");

        var userMonobank = await _monobankRepository.FirstOrDefaultAsync(m => m.UserId == userId);

        if (userMonobank != null)
        {
            if ((await IsUserTokenValidAsync(userId)).IsRight)
                return new IncorrectParametersErrorDTO(
                    "Token already added and does not need to be replaced for specified user.");

            userMonobank.MonobankToken = _encryptionService.Encrypt(monobankToken);
            await _monobankRepository.UpdateAsync(userMonobank);

            return None;
        }

        await _monobankRepository.InsertAsync(new UserMonobank
        {
            MonobankToken = _encryptionService.Encrypt(monobankToken),
            UserId = userId
        });
        return None;
    }

    public async Task<Either<ErrorDTO, List<JarDTO>>> GetJarsAsync(Guid userId)
    {
        var monobank = await _monobankRepository.FirstOrDefaultAsync(m => m.UserId == userId);
        if (monobank == null)
            return new NotFoundErrorDTO("Specified user has not authorized from Monobank yet");

        var clientInfoResult =
            await _monobankClient.GetClientInfoAsync(
                new ClientInfoRequest(_encryptionService.Decrypt(monobank.MonobankToken)));

        return clientInfoResult.Match<Either<ErrorDTO, List<JarDTO>>>(
            Right: clientInfo =>
            {
                clientInfo.Jars.ForEach(j => j.SendId = $"{_monobankConfig.SendAddress}/{j.SendId}");
                return _mapper.Map<List<JarDTO>>(clientInfo.Jars);
            },
            Left: code => new ErrorDTO(code, "Unable to get info about accounts"));
    }

    public async Task<Either<ErrorDTO, List<JarDTO>>> GetJarsAsync(List<Guid> userIds)
    {
        var jars = new List<List<JarDTO>>();
        ErrorDTO? error = null;
        foreach (var task in userIds.Select(async id => await GetJarsAsync(id)))
        {
            var result = await task;
            result.Match(
                Right: jarsList => jars.Add(jarsList),
                Left: err => error = err
            );

            if (error != null)
                return error;
        }

        return jars.SelectMany(j => j).ToList();
    }

    public async Task<Either<ErrorDTO, JarDTO>> GetJarByIdAsync(Guid userId, string jarId)
    {
        return (await GetJarsAsync(userId)).Match<Either<ErrorDTO, JarDTO>>(
            Right: jars =>
            {
                var jar = jars.FirstOrDefault(j => j.Id == jarId);
                if (jar == null)
                    return new NotFoundErrorDTO("Specified jar does not exist");

                return jar;
            },
            Left: error => error
        );
    }

    private async Task<Either<ErrorDTO, UserMonobank>> IsUserTokenValidAsync(Guid userId)
    {
        var monobank = await _monobankRepository.FirstOrDefaultAsync(m => m.UserId == userId);
        if (monobank == null)
            return new NotFoundErrorDTO("Specified user has not authorized from Monobank yet");

        var result =
            await _monobankClient.GetClientInfoAsync(
                new ClientInfoRequest(_encryptionService.Decrypt(monobank.MonobankToken)));

        if (result.IsRight)
            return monobank;

        return new ExpiredErrorDTO("Token is not valid anymore and had to be updated.");
    }
}