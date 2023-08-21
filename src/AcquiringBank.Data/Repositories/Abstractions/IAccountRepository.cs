using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Data.Models.Account;

namespace AcquiringBank.Data.Repositories.Abstractions;

public interface IAccountRepository
{
    Task InsertAccountAsync(AccountModel accountModel);
    Task<AccountModel> FindAccountByUseNameAsync(string clientName);
    Task<AccountModel> FindAccountByIdAsync(Guid id);

    Task<(int pageNumber, int pageSize, int lastPage, IEnumerable<AccountModel> items)> ListAccountsPaginatedAsync(
        int pageNumber, int pageSize);
}