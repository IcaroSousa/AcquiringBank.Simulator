using System.Collections.Generic;

namespace AcquiringBank.Domain.Pagination;

public interface IPagedResponse<TModel>
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int LastPage { get; set; }
    IEnumerable<TModel> Items { get; set; }
}