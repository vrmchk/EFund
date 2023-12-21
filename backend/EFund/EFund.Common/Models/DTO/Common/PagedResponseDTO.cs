namespace EFund.Common.Models.DTO.Common;

public class PagedResponseDTO<T>
{
    public PagedResponseDTO(ICollection<T> items)
    {
        Items = items;
        TotalCount = items.Count;
    }

    public ICollection<T> Items { get; init; }
    public int TotalCount { get; init; }
}