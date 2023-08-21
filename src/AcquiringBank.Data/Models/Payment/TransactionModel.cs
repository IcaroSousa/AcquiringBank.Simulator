using System;

namespace AcquiringBank.Data.Models.Payment;

public class TransactionModel : ModelBase
{
    public Guid CardId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Merchant { get; set; }
    public bool IsApproved { get; set; }

    public static TransactionModel Create(Guid cardId, decimal amount, string currency, string merchant,
        bool isApproved = true)
    {
        return new TransactionModel
        {
            CardId = cardId,
            Amount = amount,
            Currency = currency,
            Merchant = merchant,
            IsApproved = isApproved
        };
    }
}