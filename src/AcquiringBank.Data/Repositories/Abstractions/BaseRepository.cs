using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AcquiringBank.Data.Models;
using MongoDB.Driver;

namespace AcquiringBank.Data.Repositories.Abstractions;

public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : ModelBase
{
    protected readonly IMongoCollection<TModel> _mongoCollection;

    protected BaseRepository(IMongoDatabase mongoDatabase, string collectionName = "")
    {
        if (mongoDatabase is null)
            throw new ArgumentException("MongoDatabase could not be null");

        _mongoCollection =
            mongoDatabase.GetCollection<TModel>(collectionName == string.Empty ? nameof(TModel) : collectionName);
    }

    public async Task AddAsync(TModel model)
    {
        model.Id = Guid.NewGuid();
        model.CreatedAt = DateTime.UtcNow;

        await _mongoCollection.InsertOneAsync(model);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _mongoCollection.DeleteOneAsync(px => px.Id.Equals(id));
    }

    public async Task<TModel> FindByPredicateAsync(Expression<Func<TModel, bool>> predicate)
    {
        var cursor = await _mongoCollection.FindAsync(predicate);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TModel>> ListByPredicateAsync(Expression<Func<TModel, bool>> predicate)
    {
        var cursor = await _mongoCollection.FindAsync(predicate);
        return await cursor.ToListAsync();
    }

    public async Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<TModel>)> ListPaginateAsync(
        Expression<Func<TModel, bool>> predicate, int pageNumber, int pageSize)
    {
        var count = await _mongoCollection.CountDocumentsAsync(predicate);
        var elements = await _mongoCollection
            .Find(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize).ToListAsync();

        var totalPages = (int)count / pageSize;
        if (count % pageSize != 0) totalPages++;

        return (pageNumber, pageSize, totalPages, elements);
    }
}