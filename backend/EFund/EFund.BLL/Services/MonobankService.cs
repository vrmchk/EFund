using AutoMapper;
using EFund.BLL.Services.Cache.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Client.Monobank;
using EFund.Client.Monobank.Models.Requests;
using EFund.Client.Monobank.Models.Responses;
using EFund.Common.Enums;
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
    private readonly ICacheService _cache;

    public MonobankService(
        IMonobankClient monobankClient,
        UserManager<User> userManager,
        IRepository<UserMonobank> monobankRepository,
        IEncryptionService encryptionService,
        IMapper mapper,
        MonobankConfig monobankConfig,
        ICacheService cache)
    {
        _monobankClient = monobankClient;
        _userManager = userManager;
        _monobankRepository = monobankRepository;
        _encryptionService = encryptionService;
        _mapper = mapper;
        _monobankConfig = monobankConfig;
        _cache = cache;
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

        var cachedJarsResult = await _cache.GetAsync<ClientInfo>(CachingKey.MonobankClientInfo, userId);

        var jarsResult = await cachedJarsResult.Match<Task<Either<ErrorDTO, List<Jar>>>>(
            Some: clientInfo => Task.FromResult<Either<ErrorDTO, List<Jar>>>(clientInfo.Jars),
            None: async () =>
            {
                var clientInfoResult =
                    await _monobankClient.GetClientInfoAsync(
                        new ClientInfoRequest(_encryptionService.Decrypt(monobank.MonobankToken)));

                return await clientInfoResult.Match<Task<Either<ErrorDTO, List<Jar>>>>(
                    Right: async clientInfo =>
                    {
                        await Task.WhenAll(new[] { CachingKey.MonobankClientInfo, CachingKey.MonobankClientInfoBackup }
                            .Select(key => _cache.SetAsync(key, userId, clientInfo,
                                _monobankConfig.ClientInfoCacheSlidingLifetime,
                                _monobankConfig.ClientInfoCacheAbsoluteLifetime)));

                        return clientInfo.Jars;
                    },
                    Left: async code =>
                    {
                        var clientInfoBackup =
                            await _cache.GetAsync<ClientInfo>(CachingKey.MonobankClientInfoBackup, userId);

                        return clientInfoBackup.Match<Either<ErrorDTO, List<Jar>>>(
                            Some: clientInfo => clientInfo.Jars,
                            None: () => new ErrorDTO(code, "Unable to get info about jars")
                        );
                    }
                );
            }
        );

        return jarsResult.Match<Either<ErrorDTO, List<JarDTO>>>(
            Right: jars =>
            {
                var dtos = _mapper.Map<List<JarDTO>>(jars);

                dtos.ForEach(j => j.SendUrl = $"{_monobankConfig.SendAddress}/{j.SendUrl}");
                return dtos;
            },
            Left: error => error
        );
    }

    public async Task<Either<ErrorDTO, List<JarDTO>>> GetJarsAsync(List<Guid> userIds)
    {
        var jars = new List<List<JarDTO>>();
        ErrorDTO? error = null;
        foreach (var task in userIds.Select(async id => await GetJarsAsync(id)))
        {
            var result = await task;
            result.Match(
                Right: jars.Add,
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