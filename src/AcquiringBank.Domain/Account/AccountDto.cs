using System;

namespace AcquiringBank.Domain.Account;

public record AccountDto
{
    private AccountDto(string clientName, Guid accountId, Guid clientId, DateTime createdAt)
    {
        ClientName = clientName;
        AccountId = accountId;
        ClientId = clientId;
        CreatedAt = createdAt;
    }

    public string ClientName { get; }
    public Guid AccountId { get; }
    public Guid ClientId { get; }
    public DateTime CreatedAt { get; }

    public static AccountDto Create(string clientName, Guid accountId, Guid clientId, DateTime createdAt)
    {
        return new AccountDto(clientName, accountId, clientId, createdAt);
    }
}