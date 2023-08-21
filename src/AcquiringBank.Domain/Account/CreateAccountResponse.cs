using System;

namespace AcquiringBank.Domain.Account;

public record CreateAccountResponse(Guid ClientId, Guid AccountId);