using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Data.Repositories.Abstractions;
using MongoDB.Driver;

namespace AcquiringBank.Data.Repositories;

public class TransactionRepository : BaseRepository<TransactionModel>, ITransactionRepository
{
    public TransactionRepository(IMongoDatabase mongoDatabase, string collectionName = "CardTransactions")
        : base(mongoDatabase, collectionName)
    {
    }

    public async Task InsertPaymentTransactionAsync(TransactionModel transactionModel)
    {
        transactionModel.Id = Guid.NewGuid();
        transactionModel.CreatedAt = DateTime.UtcNow;

        await AddAsync(transactionModel);
    }

    public async Task<IEnumerable<TransactionModel>> ListTransactionsByCardId(Guid cardId)
    {
        return await ListByPredicateAsync(px => px.CardId.Equals(cardId));
    }
}