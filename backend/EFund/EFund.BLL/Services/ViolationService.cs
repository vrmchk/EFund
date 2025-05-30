using AutoMapper;
using EFund.BLL.Services.Cache.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Enums;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Violation;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class ViolationService(
    IRepository<ViolationGroup> groupRepository,
    IMapper mapper,
    ICacheService cacheService,
    IRepository<Violation> violationRepository)
    : IViolationService
{
    private readonly IRepository<ViolationGroup> _groupRepository = groupRepository;
    private readonly IRepository<Violation> _violationRepository = violationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheService;

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

    public async Task<Either<ErrorDTO, ViolationExtendedDTO>> GetByIdAsync(Guid id)
    {
        var violation = await _violationRepository.Include(v => v.ViolationGroup).FirstOrDefaultAsync(v => v.Id == id);
        if (violation == null)
            return new NotFoundErrorDTO($"Violation with ID {id} not found.");

        return _mapper.Map<ViolationExtendedDTO>(violation);
    }
}