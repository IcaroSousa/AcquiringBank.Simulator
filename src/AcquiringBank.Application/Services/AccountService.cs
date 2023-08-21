using System;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Exceptions;
using AcquiringBank.Application.Services.Mappers;
using AcquiringBank.Data.Models.Account;
using AcquiringBank.Data.Repositories.Abstractions;
using AcquiringBank.Domain.Account;
using AcquiringBank.Domain.Pagination;

namespace AcquiringBank.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(string userName)
    {
        await _accountRepository.InsertAccountAsync(new AccountModel { ClientName = userName });

        var account = await _accountRepository.FindAccountByUseNameAsync(userName);
        return new CreateAccountResponse(account.ClientId, account.Id);
    }

    public async Task<AccountDto> FindAccountByIdAsync(Guid accountId)
    {
        var accountModel = await _accountRepository.FindAccountByIdAsync(accountId);
        if (accountModel == default)
            throw new InvalidClientAccountException(accountId);

        return accountModel.ToDto();
    }

    public async Task<IPagedResponse<AccountDto>> ListAccountsPaginatedAsync(int pageNumber, int pageSize)
    {
        var pagedAccounts = await _accountRepository.ListAccountsPaginatedAsync(pageNumber, pageSize);

        return PagedResponse<AccountDto>
            .Create(
                pagedAccounts.pageNumber,
                pagedAccounts.pageSize,
                pagedAccounts.lastPage,
                pagedAccounts.items.Select(px => px.ToDto()));
    }
}