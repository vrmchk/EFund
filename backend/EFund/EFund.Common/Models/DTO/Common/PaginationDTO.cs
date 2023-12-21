namespace EFund.Common.Models.DTO.Common;

public class PaginationDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 10;
}