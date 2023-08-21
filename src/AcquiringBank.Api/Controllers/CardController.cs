using System;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Domain.Card;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AcquiringBank.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly ILogger<AccountController> _logger;

    public CardController(ILogger<AccountController> logger, ICardService cardService)
    {
        _logger = logger;
        _cardService = cardService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCardAsync([FromBody] CreateCardRequest createCardRequest)
    {
        try
        {
            var cardDetails = await _cardService.CreateACardForAClientAccount(createCardRequest.AccountId,
                createCardRequest.Limit, createCardRequest.Active);
            return Created("/api/card/v1", cardDetails);
        }
        // TODO : Create an exception handler
        catch (Exception exception)
        {
            _logger.LogError($"Ops, something went wrong when creating card to account: {createCardRequest.AccountId}",
                exception);
            return BadRequest(exception.Message);
        }
    }

    [HttpPatch("{cardId:guid}")]
    public async Task<IActionResult> UpdateCardDetailsAsync([FromRoute] Guid cardId,
        [FromBody] UpdateCardRequest updateCardRequest)
    {
        try
        {
            await _cardService.UpdateCardDetailsAsync(cardId, updateCardRequest.Limit, updateCardRequest.Active);
            return Ok();
        }
        catch (Exception exception)
        {
            _logger.LogError($"Ops, something went wrong when updating card details: {cardId}", exception);
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{cardId:guid}")]
    public async Task<IActionResult> DeleteCardAsync([FromRoute] Guid cardId)
    {
        try
        {
            await _cardService.DeleteCardAsync(cardId);
            return Ok();
        }
        catch (Exception exception)
        {
            _logger.LogError($"Ops, something went wrong when deleting card {cardId}", exception);
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{accountId:guid}")]
    public async Task<IActionResult> GetPaginatedByAccountIdAsync([FromRoute] Guid accountId,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        try
        {
            var cards = await _cardService.ListCardPaginatedByAccountId(accountId, pageNumber, pageSize);
            return Ok(cards);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Ops, something went wrong when getting cards to account {accountId}", exception);
            return BadRequest(exception.Message);
        }
    }
}