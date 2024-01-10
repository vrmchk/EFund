using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Error;
using EFund.Common.Models.DTO.Tag;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude; 

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

    public async Task<Either<ErrorDTO, TagDTO>> AddAsync(CreateTagDTO dto)
    {
        if (await _repository.AnyAsync(x => x.Name == dto.Name))
            return new AlreadyExistsErrorDTO("Tag with this name already exists.");

        var tag = _mapper.Map<Tag>(dto);
        await _repository.InsertAsync(tag);
        return _mapper.Map<TagDTO>(tag);
    }

    public async Task<List<TagDTO>> GetByNameAsync(string name)
    {
        var tags = await _repository
            .FromSqlInterpolated($"""
                SELECT T.Name
                FROM Tags T
                LEFT JOIN FundraisingTag FT ON T.Name = FT.TagsName
                WHERE T.Name LIKE {($"%{name}%")}
                GROUP BY T.Name
                ORDER BY COUNT(FT.TagsName) DESC
            """)
            .ToListAsync();

        return _mapper.Map<List<TagDTO>>(tags);
    }

    public async Task<Option<ErrorDTO>> DeleteAsync(string name)
    {
        var tag = await _repository.FirstOrDefaultAsync(x => x.Name == name);
        if (tag is null)
            return new NotFoundErrorDTO("Tag with this name does not exist");

        await _repository.DeleteAsync(tag);
        return None;
    }
}