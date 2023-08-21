using System;

namespace AcquiringBank.Application.Services.Exceptions;

public class CardNotFoundException : Exception
{
    private const string ExceptionMessage = "Invalid card details to card -> {0}";

    public CardNotFoundException(Guid cardId) : base(string.Format(ExceptionMessage, cardId))
    {
    }

    public CardNotFoundException(string cardNumber) : base(string.Format(ExceptionMessage, cardNumber))
    {
    }
}