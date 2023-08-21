using System;
using System.Collections.Generic;

namespace AcquiringBank.Domain.Pagination;

public class PagedResponse<TModel> : IPagedResponse<TModel>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int LastPage { get; set; }
    public IEnumerable<TModel> Items { get; set; } = ArraySegment<TModel>.Empty;

    public static PagedResponse<TModel> Create(int pageNumber, int pageSize, int lastPage, IEnumerable<TModel> items)
    {
        return new PagedResponse<TModel>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            LastPage = lastPage,
            Items = items
        };
    }
}