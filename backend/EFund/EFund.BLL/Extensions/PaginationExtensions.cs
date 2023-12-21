using EFund.BLL.Utility;
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
}