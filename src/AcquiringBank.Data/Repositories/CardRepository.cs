using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Card;
using AcquiringBank.Data.Repositories.Abstractions;
using MongoDB.Driver;

namespace AcquiringBank.Data.Repositories;

public class CardRepository : BaseRepository<CardModel>, ICardRepository
{
    public CardRepository(IMongoDatabase mongoDatabase, string collectionName = "Cards")
        : base(mongoDatabase, collectionName)
    {
    }

    public async Task InsertCardAsync(CardModel cardModel)
    {
        cardModel.Id = Guid.NewGuid();
        cardModel.CreatedAt = DateTime.UtcNow;

        await AddAsync(cardModel);
    }

    public async Task UpdateCardDetailsAsync(Guid cardId, decimal limit, bool active)
    {
        var filterDefinition = Builders<CardModel>
            .Filter
            .Eq(px => px.Id, cardId);

        var updateDefinition = Builders<CardModel>
            .Update
            .Set(px => px.Limit, limit)
            .Set(px => px.IsActive, active)
            .Set(px => px.UpdatedAt, DateTime.UtcNow);

        await _mongoCollection.UpdateOneAsync(filterDefinition, updateDefinition);
    }

    public async Task DeleteCardAsync(Guid cardId)
    {
        await DeleteByIdAsync(cardId);
    }

    public async Task<CardModel> FindCardByIdAsync(Guid cardId)
    {
        return await FindByPredicateAsync(px => px.Id.Equals(cardId));
    }

    public async Task<CardModel> FindCardByNumberAndVerificationValue(string number, string verificationValue)
    {
        return await FindByPredicateAsync(px =>
            px.Number.Equals(number) && px.VerificationValue.Equals(verificationValue));
    }

    public async Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<CardModel> items)>
        ListCardsPaginatedByAccountIdAsync(Guid accountId, int pageNumber, int pageSize)
    {
        return await ListPaginateAsync(px => px.AccountId.Equals(accountId), pageNumber, pageSize);
    }
}