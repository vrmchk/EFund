using AutoMapper;
using EFund.BLL.Services.Cache.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Violation;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class ViolationService : IViolationService
{
    private readonly IRepository<ViolationGroup> _groupRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public ViolationService(IRepository<ViolationGroup> groupRepository, IMapper mapper, ICacheService cacheService)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<List<ViolationGroupDTO>> GetGroupedViolationsAsync(bool withDeleted)
    {
        var groups = await _cacheService.GetOrSetAsync(
            CachingKey.ViolationGroups,
            withDeleted,
            async () => await _groupRepository
                .Where(g => withDeleted || !g.IsDeleted)
                .Include(g => g.Violations.Where(v => withDeleted || !v.IsDeleted))
                .ToListAsync()
        );

        return _mapper.Map<List<ViolationGroupDTO>>(groups);
    }
}