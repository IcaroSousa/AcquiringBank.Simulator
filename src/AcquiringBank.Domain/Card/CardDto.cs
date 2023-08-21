using System;

namespace AcquiringBank.Domain.Card;

public record CardDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; }
    public string Number { get; set; }
    public string VerificationValue { get; set; }
    public Expiry ExpiresAt { get; set; }
    public decimal Limit { get; set; }
    public string FriendlyName { get; set; }
    public bool IsActive { get; set; }

    public static CardDto Create(Guid id, string clientName, string number, string verificationValue, int month,
        int year,
        decimal limit, string friendlyName, bool isActive = true)
    {
        return new CardDto
        {
            Id = id,
            ClientName = clientName,
            Number = number,
            VerificationValue = verificationValue,
            ExpiresAt = new Expiry { Month = month, Year = year },
            Limit = limit,
            FriendlyName = friendlyName,
            IsActive = isActive
        };
    }
}

public record Expiry
{
    public int Year { get; set; }
    public int Month { get; set; }
}