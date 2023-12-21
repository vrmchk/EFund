namespace EFund.BLL.Utility;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        TotalItems = totalItems;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        AddRange(items);
    }

    public int PageNumber { get; private set; }

    public int TotalPages { get; private set; }

    public int PageSize { get; private set; }

    public int TotalItems { get; private set; }

    public bool HasPrevious => PageNumber > 1;

    public bool HasNext => PageNumber < TotalPages;
}