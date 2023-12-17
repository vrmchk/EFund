namespace EFund.BLL.Utility;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int totalPages, int pageNumber, int pageSize)
    {
        TotalTotalPages = totalPages;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalPages / (double)pageSize);
        AddRange(items);
    }

    public int CurrentPage { get; private set; }

    public int TotalPages { get; private set; }

    public int PageSize { get; private set; }

    public int TotalTotalPages { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;
}