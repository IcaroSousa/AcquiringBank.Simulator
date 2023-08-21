using System;

namespace AcquiringBank.Data.Models.Account;

public class AccountModel : ModelBase
{
    public Guid ClientId { get; set; } = Guid.NewGuid();
    public string ClientName { get; set; }
}