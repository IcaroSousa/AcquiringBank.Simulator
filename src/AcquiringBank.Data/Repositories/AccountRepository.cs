using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Account;
using AcquiringBank.Data.Repositories.Abstractions;
using MongoDB.Driver;

namespace AcquiringBank.Data.Repositories;

public class AccountRepository : BaseRepository<AccountModel>, IAccountRepository
{
    public AccountRepository(IMongoDatabase mongoDatabase, string collectionName = "Accounts")
        : base(mongoDatabase, collectionName)
    {
    }

    public async Task InsertAccountAsync(AccountModel accountModel)
    {
        await AddAsync(accountModel);
    }

    public async Task<AccountModel> FindAccountByUseNameAsync(string clientName)
    {
        return await FindByPredicateAsync(px => px.ClientName.Equals(clientName));
    }

    public async Task<AccountModel> FindAccountByIdAsync(Guid id)
    {
        return await FindByPredicateAsync(px => px.Id.Equals(id));
    }

    public async Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<AccountModel> items)>
        ListAccountsPaginatedAsync(int pageNumber, int pageSize)
    {
        return await ListPaginateAsync(px => px.ClientName != string.Empty, pageNumber, pageSize);
    }
}