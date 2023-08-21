using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AcquiringBank.Data.Models;

namespace AcquiringBank.Data.Repositories.Abstractions;

public interface IBaseRepository<TModel> where TModel : ModelBase
{
    Task AddAsync(TModel model);
    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<TModel>> ListByPredicateAsync(Expression<Func<TModel, bool>> predicate);
    Task<TModel> FindByPredicateAsync(Expression<Func<TModel, bool>> predicate);

    Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<TModel>)> ListPaginateAsync(
        Expression<Func<TModel, bool>> predicate, int pageNumber, int pageSize);
}