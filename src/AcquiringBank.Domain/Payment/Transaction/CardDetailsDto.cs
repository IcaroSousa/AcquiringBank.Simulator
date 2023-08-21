namespace AcquiringBank.Domain.Payment.Transaction;

public struct CardDetailsDto
{
    public string Number { get; set; }
    public string VerificationValue { get; set; }
    public Expiry ExpiresAt { get; set; }

    public struct Expiry
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}