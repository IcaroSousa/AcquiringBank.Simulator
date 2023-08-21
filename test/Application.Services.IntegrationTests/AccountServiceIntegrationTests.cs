using System;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Data.Repositories;
using Application.Services.IntegrationTests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Application.Services.IntegrationTests;

public class AccountServiceIntegrationTests : IClassFixture<MongoDbFixture>, IDisposable
{
    private const string AccountCollectionName = "Accounts";

    private readonly IAccountService _accountService;
    private readonly IMongoDatabase _mongoDatabase;

    public AccountServiceIntegrationTests(MongoDbFixture mongoDbFixture)
    {
        _mongoDatabase = mongoDbFixture.MongoDatabase;
        var accountRepository = new AccountRepository(_mongoDatabase);

        _accountService = new AccountService(accountRepository);
    }

    public void Dispose()
    {
        _mongoDatabase.DropCollection(AccountCollectionName);
    }

    [Fact]
    public async Task WhenTryToCreateAnAccountToAClientItShouldReturnAValidResponse()
    {
        // Setup
        const string clientName = "John Doe";

        // Act
        var accountDetails = await _accountService.CreateAccountAsync(clientName);

        // Assert
        accountDetails
            .Should()
            .NotBeNull();
    }

    [Theory]
    [InlineData(1, 10, 10)]
    [InlineData(1, 5, 5)]
    [InlineData(2, 10, 0)]
    [InlineData(2, 9, 1)]
    public async Task WhenTryToListAccountsPaginatedItShouldReturnAValidPaginatedResponse(int pageNumber, int pageSize,
        int expectedItems)
    {
        // Setup
        const string clientName = "John Doe";

        // Act
        for (var index = 0; index < 10; index++) 
            await _accountService.CreateAccountAsync($"{clientName}-{index}");

        var paginatedResponse = await _accountService.ListAccountsPaginatedAsync(pageNumber, pageSize);

        paginatedResponse
            .Items
            .Count()
            .Should()
            .Be(expectedItems);
    }
}