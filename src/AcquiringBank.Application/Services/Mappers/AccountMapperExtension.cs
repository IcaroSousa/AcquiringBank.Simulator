using AcquiringBank.Data.Models.Account;
using AcquiringBank.Domain.Account;

namespace AcquiringBank.Application.Services.Mappers;

public static class AccountMapperExtension
{
    public static AccountDto ToDto(this AccountModel accountModel)
    {
        return AccountDto.Create(accountModel.ClientName,
            accountModel.Id,
            accountModel.ClientId,
            accountModel.CreatedAt);
    }
}