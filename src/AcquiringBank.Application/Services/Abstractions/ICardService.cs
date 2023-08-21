using System;
using System.Threading.Tasks;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Pagination;

namespace AcquiringBank.Application.Services.Abstractions;

public interface ICardService
{
    Task<CardDto> CreateACardForAClientAccount(Guid accountId, decimal cardLimit, bool active = true);
    Task UpdateCardDetailsAsync(Guid cardId, decimal cardLimit, bool active);
    Task DeleteCardAsync(Guid cardId);
    Task<CardDto> FindCardDetailsByNumberAndVerificationValue(string number, string verificationValue);

    Task<IPagedResponse<CardDto>> ListCardPaginatedByAccountId(Guid accountId, int pageNumber, int pageSize);
}