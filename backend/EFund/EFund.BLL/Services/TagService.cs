using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Tag;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class TagService : ITagService
{
    private readonly IRepository<Tag> _repository;
    private readonly IMapper _mapper;

    public TagService(IRepository<Tag> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TagDTO>> GetAllAsync(PaginationDTO pagination)
    {
        var tags = await _repository
            .FromSqlInterpolated($"""
                SELECT T.Name
                FROM Tags T
                LEFT JOIN FundraisingTag FT ON T.Name = FT.TagsName
                GROUP BY T.Name
                ORDER BY COUNT(FT.TagsName) DESC
                OFFSET {pagination.PageSize * (pagination.PageNumber - 1)} ROWS
                FETCH NEXT {pagination.PageSize} ROWS ONLY
            """)
            .ToListAsync();

        return _mapper.Map<List<TagDTO>>(tags);
    }

    public async Task<Either<ErrorDTO, TagDTO>> AddAsync(CreateTagDTO dto)
    {
        if (await _repository.AnyAsync(x => x.Name == dto.Name))
            return new AlreadyExistsErrorDTO("Tag with this name already exists.");

        var tag = _mapper.Map<Tag>(dto);
        await _repository.InsertAsync(tag);
        return _mapper.Map<TagDTO>(tag);
    }

    public async Task<List<TagDTO>> GetByNameAsync(string name, PaginationDTO pagination)
    {
        var tags = await _repository
            .FromSqlInterpolated($"""
                SELECT T.Name
                FROM Tags T
                LEFT JOIN FundraisingTag FT ON T.Name = FT.TagsName
                WHERE T.Name LIKE {($"%{name}%")}
                GROUP BY T.Name
                ORDER BY COUNT(FT.TagsName) DESC
                OFFSET {pagination.PageSize * (pagination.PageNumber - 1)} ROWS
                FETCH NEXT {pagination.PageSize} ROWS ONLY
            """)
            .ToListAsync();

        return _mapper.Map<List<TagDTO>>(tags);
    }
}