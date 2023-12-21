﻿namespace EFund.Common.Models.DTO.Common;

public class PagedListDTO<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public bool HasPrevious { get; set; }

    public bool HasNext { get; set; }
}