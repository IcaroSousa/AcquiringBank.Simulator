using System;

namespace AcquiringBank.Application.Services.Exceptions;

public class InvalidLimitValueException : Exception
{
    private const string ExceptionMessage = "Card limit should not be a negative value!";

    public InvalidLimitValueException() : base(ExceptionMessage)
    {
    }
}