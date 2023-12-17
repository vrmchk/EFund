namespace EFund.Common.Models.DTO;

public class PaginationDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 10;
}