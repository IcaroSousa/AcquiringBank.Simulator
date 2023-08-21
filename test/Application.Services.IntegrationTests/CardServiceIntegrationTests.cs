using System;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Exceptions;
using AcquiringBank.Data.Repositories;
using Application.Services.IntegrationTests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Application.Services.IntegrationTests;

public class CardServiceIntegrationTests : IClassFixture<MongoDbFixture>, IDisposable
{
    private const string AccountCollectionName = "Accounts";
    private const string CardCollectionName = "Cards";
    private const string ClientName = "John Doe";

    private readonly IAccountService _accountService;
    private readonly ICardService _cardService;

    private readonly IMongoDatabase _mongoDatabase;

    public CardServiceIntegrationTests(MongoDbFixture mongoDbFixture)
    {
        _mongoDatabase = mongoDbFixture.MongoDatabase;
        var accountRepository = new AccountRepository(_mongoDatabase);
        var cardRepository = new CardRepository(_mongoDatabase);

        _accountService = new AccountService(accountRepository);
        _cardService = new CardService(_accountService, cardRepository);
    }

    public void Dispose()
    {
        _mongoDatabase.DropCollection(AccountCollectionName);
        _mongoDatabase.DropCollection(CardCollectionName);
    }


    [Fact]
    public async Task WhenTryToCreateACardToAValidClientAccountItShouldReturnValidCardDetails()
    {
        // Setup
        var accountDetails = await _accountService.CreateAccountAsync(ClientName);

        // Act
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, 1000);

        // Assert
        cardDetails
            .Should()
            .NotBeNull();

        cardDetails
            .ClientName
            .Should()
            .Be(ClientName);
    }

    [Fact]
    public async Task WhenTryToCreateACardToAnInvalidClientAccountItShouldReturnInvalidClientAccountException()
    {
        // Setup
        const decimal cardLimit = 1000;

        // Act
        // Assert
        await Assert.ThrowsAsync<InvalidClientAccountException>(async () =>
            await _cardService.CreateACardForAClientAccount(Guid.Empty, cardLimit));
    }

    [Fact]
    public async Task
        WhenTryToCreateACardWithNegativeLimitToAValidAccountClientItShouldReturnInvalidLimitValueException()
    {
        // Setup
        const decimal cardLimit = -10;
        var accountDetails = await _accountService.CreateAccountAsync(ClientName);

        // Act
        // Assert
        await Assert.ThrowsAsync<InvalidLimitValueException>(async () =>
            await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, cardLimit));
    }

    [Fact]
    public async Task WhenTryToUpdateDetailsFromAnInvalidCardItShouldReturnCardNotFoundException()
    {
        // Setup
        const decimal cardLimit = 1000;

        // Act
        // Assert
        await Assert.ThrowsAsync<CardNotFoundException>(async () =>
            await _cardService.UpdateCardDetailsAsync(Guid.Empty, cardLimit, true));
    }

    [Fact]
    public async Task WhenTryToUpdateTheStatusOfAValidCardItShouldBeSuccess()
    {
        // Setup
        const decimal cardLimit = 1000;

        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, cardLimit);

        // Act
        await _cardService.UpdateCardDetailsAsync(cardDetails.Id, cardLimit, false);
        var updatedCard = await _cardService.ListCardPaginatedByAccountId(accountDetails.AccountId, 1, 1);

        // Assert
        updatedCard
            .Items
            .First()
            .IsActive
            .Should()
            .Be(false);

        cardDetails
            .IsActive
            .Should()
            .NotBe(updatedCard.Items.First().IsActive);
    }

    [Fact]
    public async Task WhenTryToUpdateTheLimitOfAValidCardIdShouldBeSuccess()
    {
        // Setup
        const decimal cardLimit = 1000;
        const decimal newCardLimit = 3300;

        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, cardLimit);

        // Act
        await _cardService.UpdateCardDetailsAsync(cardDetails.Id, newCardLimit, true);
        var updatedCard = await _cardService.ListCardPaginatedByAccountId(accountDetails.AccountId, 1, 1);

        // Assert
        updatedCard
            .Items
            .First()
            .Limit
            .Should()
            .Be(newCardLimit);
    }

    [Fact]
    public async Task WhenTryToUpdateTheLimitOfAValidCardToANegativeValueItShouldReturnInvalidLimitValueException()
    {
        // Setup
        const decimal cardLimit = 1000;
        const decimal newCardLimit = -1000;

        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, cardLimit);

        // Act
        // Assert
        await Assert.ThrowsAsync<InvalidLimitValueException>(async () =>
            await _cardService.UpdateCardDetailsAsync(cardDetails.Id, newCardLimit, true));
    }

    [Fact]
    public async Task WhenTryToDeleteAValidCardItShouldBeSuccess()
    {
        // Setup
        const decimal cardLimit = 1000;

        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, cardLimit);

        // Act
        await _cardService.DeleteCardAsync(cardDetails.Id);
        var updatedCard = await _cardService.ListCardPaginatedByAccountId(accountDetails.AccountId, 1, 10);

        // Assert
        updatedCard
            .Items
            .Should()
            .BeEmpty();
    }
}