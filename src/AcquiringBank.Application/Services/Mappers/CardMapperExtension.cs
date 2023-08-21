using AcquiringBank.Data.Models.Card;
using AcquiringBank.Domain.Card;

namespace AcquiringBank.Application.Services.Mappers;

public static class CardMapperExtension
{
    public static CardDto ToDto(this CardModel cardModel, string clientName)
    {
        return CardDto.Create(
            cardModel.Id,
            clientName,
            cardModel.Number,
            cardModel.VerificationValue,
            cardModel.ExpiryMonth,
            cardModel.ExpiryYear,
            cardModel.Limit,
            cardModel.FriendlyName,
            cardModel.IsActive);
    }
}