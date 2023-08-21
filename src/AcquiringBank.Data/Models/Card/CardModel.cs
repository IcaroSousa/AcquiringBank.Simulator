using System;

namespace AcquiringBank.Data.Models.Card;

public class CardModel : ModelBase
{
    public Guid AccountId { get; set; }
    public string Number { get; set; } // Should be encrypted
    public string VerificationValue { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public decimal Limit { get; set; }
    public string FriendlyName { get; set; }
    public bool IsActive { get; set; }
}