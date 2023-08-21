using System;
using System.Threading.Tasks;
using AcquiringBank.Domain.Account;
using AcquiringBank.Domain.Pagination;

namespace AcquiringBank.Application.Services.Abstractions;

public interface IAccountService
{
    Task<CreateAccountResponse> CreateAccountAsync(string userName);
    Task<AccountDto> FindAccountByIdAsync(Guid accountId);
    Task<IPagedResponse<AccountDto>> ListAccountsPaginatedAsync(int pageNumber, int pageSize);
}