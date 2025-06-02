using EFund.BLL.Utility;
using EFund.Common.Models.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Extensions;

public static class PaginationExtensions
{
    public static PagedList<T> ToPagedList<T>(this ICollection<T> source, int pageNumber, int pageSize)
    {
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, source.Count, pageNumber, pageSize);
    }

    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, source.Count(), pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, source.Count(), pageNumber, pageSize);
    }
    
    public static PagedListDTO<T> ToDto<T>(this PagedList<T> pagedList)
    {
        return new PagedListDTO<T>
        {
            Items = pagedList,
            Page = pagedList.Page,
            TotalPages = pagedList.TotalPages,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            HasPrevious = pagedList.HasPrevious,
            HasNext = pagedList.HasNext
        };
    }
}