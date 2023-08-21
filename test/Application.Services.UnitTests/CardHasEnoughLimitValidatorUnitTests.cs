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

public class CardHasEnoughLimitValidatorUnitTests
{
    private readonly IPaymentTransactionValidator _enoughLimitValidator = new CardHasEnoughLimitValidator();

    [Fact]
    public void WhenTryToValidateATransactionWithACardWithInsufficientLimitItShouldReturnSuccessFalse()
    {
        // Setup
        const string responseMessage = "Card has no limit to approve this transaction!";

        var cardDetails = CardDto
            .Create(
                Guid.NewGuid(),
                "John Doe",
                "5893927441054887517",
                "541",
                12,
                2022,
                20,
                "Home, Books & Outdoors");

        var processedTransactions = new List<TransactionModel>
        {
            TransactionModel.Create(cardDetails.Id, 10, "EUR", "Burger King")
        };

        // Act
        var transactionValidationResult =
            _enoughLimitValidator.ValidateTransaction(cardDetails, processedTransactions,
                TransactionRequest(cardDetails));

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
    public void WhenTryToValidateATransactionWithACardWithSufficientLimitItShouldReturnSuccessTrue()
    {
        {
            // Setup

            var cardDetails = CardDto
                .Create(
                    Guid.NewGuid(),
                    "John Doe",
                    "5893927441054887517",
                    "541",
                    12,
                    2022,
                    1000,
                    "Home, Books & Outdoors");

            // Act
            var transactionValidationResult = _enoughLimitValidator.ValidateTransaction(cardDetails,
                new List<TransactionModel>(),
                TransactionRequest(cardDetails));

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
    }

    private static PaymentTransactionRequest TransactionRequest(CardDto cardDetails)
    {
        return new PaymentTransactionRequest
        {
            Details = new TransactionDetailsDto
            {
                Amount = (decimal)12.99,
                Currency = "EUR",
                Merchant = "Dunkin Donuts",
                SubmittedAt = DateTime.UtcNow
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