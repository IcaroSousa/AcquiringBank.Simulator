using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Mappers;
using AcquiringBank.Application.Services.Validators;
using AcquiringBank.Data.Repositories.Abstractions;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly ICardService _cardService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEnumerable<IPaymentTransactionValidator> _validationRules;

    public PaymentService(ITransactionRepository transactionRepository, ICardService cardService,
        IEnumerable<IPaymentTransactionValidator> validationRules)
    {
        _transactionRepository = transactionRepository;
        _cardService = cardService;
        _validationRules = validationRules;
    }

    public async Task<PaymentTransactionResponse> ProcessTransaction(
        PaymentTransactionRequest paymentTransactionRequest)
    {
        try
        {
            var cardDetails = await _cardService.FindCardDetailsByNumberAndVerificationValue(
                paymentTransactionRequest.CardDetails.Number, 
                paymentTransactionRequest.CardDetails.VerificationValue);

            var paymentTransactionResponse = await ValidateTransactionAsync(paymentTransactionRequest, cardDetails);
            if (!paymentTransactionResponse.IsApproved)
                return paymentTransactionResponse;

            // TODO: Check if could be a good practice store failed transactions
            var transactionModel = paymentTransactionRequest.ToModel(cardDetails.Id);
            await _transactionRepository.InsertPaymentTransactionAsync(transactionModel);
            return paymentTransactionResponse;
        }
        catch (Exception exception)
        {
            return new PaymentTransactionResponse
            {
                Validations = new List<TransactionValidationResult>
                {
                    TransactionValidationResult.FromFail(exception.Message)
                }
            };
        }
    }

    private async Task<PaymentTransactionResponse> ValidateTransactionAsync(
        PaymentTransactionRequest paymentTransactionRequest, CardDto cardDetails)
    {
        var paymentTransactionResponse = new PaymentTransactionResponse();
        var transactionModels = await _transactionRepository.ListTransactionsByCardId(cardDetails.Id);

        foreach (var transactionRule in _validationRules)
        {
            var validation =
                transactionRule.ValidateTransaction(cardDetails, transactionModels, paymentTransactionRequest);

            if (!validation.Success)
                paymentTransactionResponse
                    .Validations
                    .Add(validation);
        }

        return paymentTransactionResponse;
    }
}