using System;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Exceptions;
using AcquiringBank.Application.Services.Mappers;
using AcquiringBank.Data.Models.Card;
using AcquiringBank.Data.Repositories.Abstractions;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Pagination;
using Bogus;

namespace AcquiringBank.Application.Services;

public class CardService : ICardService
{
    private readonly IAccountService _accountService;
    private readonly ICardRepository _cardRepository;

    public CardService(IAccountService accountService, ICardRepository cardRepository)
    {
        _accountService = accountService;
        _cardRepository = cardRepository;
    }

    public async Task<CardDto> CreateACardForAClientAccount(Guid accountId, decimal cardLimit, bool active = true)
    {
        if (cardLimit < 0)
            throw new InvalidLimitValueException();

        var account = await _accountService.FindAccountByIdAsync(accountId);
        var cardModel = CardDetailsFaker(cardLimit, active).Generate();
        cardModel.AccountId = accountId;

        await _cardRepository.InsertCardAsync(cardModel);
        return cardModel.ToDto(account.ClientName);
    }

    public async Task UpdateCardDetailsAsync(Guid cardId, decimal cardLimit, bool active)
    {
        if (cardLimit < 0)
            throw new InvalidLimitValueException();

        var card = await _cardRepository.FindCardByIdAsync(cardId);
        if (card == default)
            throw new CardNotFoundException(cardId);

        await _cardRepository.UpdateCardDetailsAsync(cardId, cardLimit, active);
    }

    public async Task DeleteCardAsync(Guid cardId)
    {
        await _cardRepository.DeleteCardAsync(cardId);
    }

    public async Task<CardDto> FindCardDetailsByNumberAndVerificationValue(string number, string verificationValue)
    {
        var cardModel = await _cardRepository.FindCardByNumberAndVerificationValue(number, verificationValue);
        if (cardModel is null)
            throw new CardNotFoundException(number);

        var account = await _accountService.FindAccountByIdAsync(cardModel.AccountId);

        return cardModel.ToDto(account.ClientName);
    }

    public async Task<IPagedResponse<CardDto>> ListCardPaginatedByAccountId(Guid accountId, int pageNumber,
        int pageSize)
    {
        var account = await _accountService.FindAccountByIdAsync(accountId);
        var pagedCards = await _cardRepository.ListCardsPaginatedByAccountIdAsync(accountId, pageNumber, pageSize);

        return PagedResponse<CardDto>
            .Create(
                pagedCards.pageNumber,
                pagedCards.pageSize,
                pagedCards.lastPage,
                pagedCards.items.Select(px => px.ToDto(account.ClientName)));
    }

    // Using bogus to generate fake card details
    private static Faker<CardModel> CardDetailsFaker(decimal limit, bool active)
    {
        return new Faker<CardModel>()
            .RuleFor(px => px.Number, faker => faker.Finance.CreditCardNumber())
            .RuleFor(px => px.VerificationValue, faker => faker.Finance.CreditCardCvv())
            .RuleFor(px => px.ExpiryMonth, faker => faker.Date.Future().Month)
            .RuleFor(px => px.ExpiryYear, faker => faker.Date.Future(2).Year)
            .RuleFor(px => px.FriendlyName, faker => faker.Commerce.Department())
            .RuleFor(px => px.Limit, limit)
            .RuleFor(px => px.IsActive, active);
    }
}