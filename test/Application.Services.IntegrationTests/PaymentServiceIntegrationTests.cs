using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Validators;
using AcquiringBank.Data.Repositories;
using AcquiringBank.Domain.Payment;
using AcquiringBank.Domain.Payment.Transaction;
using Application.Services.IntegrationTests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Application.Services.IntegrationTests;

public class PaymentServiceIntegrationTests : IClassFixture<MongoDbFixture>, IDisposable
{
    private const string AccountCollectionName = "Accounts";
    private const string CardCollectionName = "Cards";
    private const string CardTransactionsCollectionName = "CardTransactions";
    private const string ClientName = "John Doe";

    private readonly IAccountService _accountService;
    private readonly ICardService _cardService;

    private readonly IMongoDatabase _mongoDatabase;
    private readonly IPaymentService _paymentService;

    public PaymentServiceIntegrationTests(MongoDbFixture mongoDbFixture)
    {
        _mongoDatabase = mongoDbFixture.MongoDatabase;
        var accountRepository = new AccountRepository(_mongoDatabase);
        var cardRepository = new CardRepository(_mongoDatabase);
        var transactionRepository = new TransactionRepository(_mongoDatabase);


        _accountService = new AccountService(accountRepository);
        _cardService = new CardService(_accountService, cardRepository);

        _paymentService = new PaymentService(transactionRepository, _cardService, new List<IPaymentTransactionValidator>
        {
            new CardIsActiveValidator()
        });
    }

    public void Dispose()
    {
        _mongoDatabase.DropCollection(AccountCollectionName);
        _mongoDatabase.DropCollection(CardCollectionName);
        _mongoDatabase.DropCollection(CardTransactionsCollectionName);
    }

    [Fact]
    public async Task WhenTryToProcessATransactionWithAnActiveCardItShouldBeSuccess()
    {
        // Setup
        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, 1000);

        var paymentRequest = new PaymentTransactionRequest
        {
            Details = new TransactionDetailsDto
            {
                Amount = (decimal)12.99,
                Currency = "EUR",
                Merchant = "Dunkin Donuts",
                SubmittedAt = DateTime.UtcNow
            },
            CardDetails = new CardDetailsDto
            {
                Number = cardDetails.Number,
                VerificationValue = cardDetails.VerificationValue,
                ExpiresAt = new CardDetailsDto.Expiry
                    { Month = cardDetails.ExpiresAt.Month, Year = cardDetails.ExpiresAt.Year }
            }
        };

        // Act
        var response = await _paymentService.ProcessTransaction(paymentRequest);

        // Assert
        response
            .IsApproved
            .Should()
            .BeTrue();

        response
            .Validations
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task WhenTryToProcessATransactionWithAnNonActiveCardItShouldBeSuccess()
    {
        // Setup
        const string responseMessage = "Card is not active!";

        var accountDetails = await _accountService.CreateAccountAsync(ClientName);
        var cardDetails = await _cardService.CreateACardForAClientAccount(accountDetails.AccountId, 1000, false);

        var paymentRequest = new PaymentTransactionRequest
        {
            Details = new TransactionDetailsDto
            {
                Amount = (decimal)12.99,
                Currency = "EUR",
                Merchant = "Dunkin Donuts",
                SubmittedAt = DateTime.UtcNow
            },
            CardDetails = new CardDetailsDto
            {
                Number = cardDetails.Number,
                VerificationValue = cardDetails.VerificationValue,
                ExpiresAt = new CardDetailsDto.Expiry
                    { Month = cardDetails.ExpiresAt.Month, Year = cardDetails.ExpiresAt.Year }
            }
        };

        // Act
        var response = await _paymentService.ProcessTransaction(paymentRequest);

        // Assert
        response
            .IsApproved
            .Should()
            .BeFalse();

        response
            .Validations
            .First()
            .Message
            .Should()
            .Be(responseMessage);
    }
}