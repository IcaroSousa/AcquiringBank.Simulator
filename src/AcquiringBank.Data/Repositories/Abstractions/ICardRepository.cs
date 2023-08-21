using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Card;

namespace AcquiringBank.Data.Repositories.Abstractions;

public interface ICardRepository
{
    Task InsertCardAsync(CardModel cardModel);
    Task UpdateCardDetailsAsync(Guid cardId, decimal limit, bool active);
    Task DeleteCardAsync(Guid cardId);

    Task<CardModel> FindCardByIdAsync(Guid cardId);
    Task<CardModel> FindCardByNumberAndVerificationValue(string number, string verificationValue);

    Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<CardModel> items)> ListCardsPaginatedByAccountIdAsync(
        Guid accountId, int pageNumber, int pageSize);
}