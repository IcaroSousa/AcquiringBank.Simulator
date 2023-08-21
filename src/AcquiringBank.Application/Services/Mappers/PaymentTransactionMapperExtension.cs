using System;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Mappers;

public static class PaymentTransactionMapperExtension
{
    public static TransactionModel ToModel(this PaymentTransactionRequest paymentTransactionRequest, Guid cardId,
        bool isApproved = true)
    {
        return TransactionModel
            .Create(
                cardId,
                paymentTransactionRequest.Details.Amount,
                paymentTransactionRequest.Details.Currency,
                paymentTransactionRequest.Details.Merchant,
                isApproved);
    }
}