using System;
using System.Collections.Generic;
using AcquiringBank.Application.Services.Validators;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;
using AcquiringBank.Domain.Payment.Transaction;
using FluentAssertions;
using Xunit;

namespace Application.Services.UnitTests;

public class CardIsExpiredValidatorUnitTests
{
    private readonly IPaymentTransactionValidator _cardExpiredValidator = new CardIsExpiredValidator();

    [Fact]
    public void WhenTryToValidateATransactionForAExpiredCardItShouldReturnSuccessFalse()
    {
        // Setup
        const string responseMessage = "Card is expired!";
        var transactionDate = new DateTime(2022, 04, 01);
        var cardExpiryDate = new DateOnly(2022, 01, 01);


        var cardDetails = CardDto
            .Create(
                Guid.NewGuid(),
                "John Doe",
                "5893927441054887517",
                "541",
                cardExpiryDate.Month,
                cardExpiryDate.Year,
                1000,
                "Home, Books & Outdoors");

        // Act
        var transactionValidationResult = _cardExpiredValidator.ValidateTransaction(
            cardDetails,
            new List<TransactionModel>(),
            TransactionRequest(cardDetails, transactionDate));

        // Assert
        transactionValidationResult
            .Success
            .Should()
            .BeFalse();

        transactionValidationResult
            .Message
            .Should()
            .Be(responseMessage);
    }

    [Fact]
    public void WhenTryToValidateATransactionForAValidCardCardItShouldReturnSuccessTrue()
    {
        // Setup
        var transactionDate = new DateTime(2022, 01, 01);
        var cardExpiryDate = new DateOnly(2022, 01, 01);

        var cardDetails = CardDto
            .Create(
                Guid.NewGuid(),
                "John Doe",
                "5893927441054887517",
                "541",
                cardExpiryDate.Month,
                cardExpiryDate.Year,
                1000,
                "Home, Books & Outdoors");

        // Act
        var transactionValidationResult = _cardExpiredValidator.ValidateTransaction(
            cardDetails,
            new List<TransactionModel>(),
            TransactionRequest(cardDetails, transactionDate));

        // Assert
        transactionValidationResult
            .Success
            .Should()
            .BeTrue();

        transactionValidationResult
            .Message
            .Should()
            .BeNull();
    }

    private static PaymentTransactionRequest TransactionRequest(CardDto cardDetails, DateTime transactionDate)
    {
        return new PaymentTransactionRequest
        {
            Details = new TransactionDetailsDto
            {
                Amount = (decimal)12.99,
                Currency = "EUR",
                Merchant = "Dunkin Donuts",
                SubmittedAt = transactionDate
            },
            CardDetails = new CardDetailsDto
            {
                Number = cardDetails.Number,
                VerificationValue = cardDetails.VerificationValue,
                ExpiresAt = new CardDetailsDto.Expiry
                    { Month = cardDetails.ExpiresAt.Month, Year = cardDetails.ExpiresAt.Year }
            }
        };
    }
}