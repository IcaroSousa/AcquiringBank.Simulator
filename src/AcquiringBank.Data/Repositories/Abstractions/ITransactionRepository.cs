using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Payment;

namespace AcquiringBank.Data.Repositories.Abstractions;

public interface ITransactionRepository
{
    public Task InsertPaymentTransactionAsync(TransactionModel transactionModel);
    public Task<IEnumerable<TransactionModel>> ListTransactionsByCardId(Guid cardId);
}