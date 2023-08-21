using System;

namespace AcquiringBank.Application.Services.Exceptions;

public class InvalidClientAccountException : Exception
{
    private const string ExceptionMessage = "Invalid client account -> {0}";

    public InvalidClientAccountException(Guid accountId) : base(string.Format(ExceptionMessage, accountId))
    {
    }
}