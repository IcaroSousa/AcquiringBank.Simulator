using System;

namespace AcquiringBank.Domain.Card;

public record CreateCardRequest(Guid AccountId, decimal Limit, bool Active = true);