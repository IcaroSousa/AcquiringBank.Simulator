using System;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Domain.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AcquiringBank.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
    {
        _logger = logger;
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessTransactionAsync(
        [FromBody] PaymentTransactionRequest paymentTransactionRequest)
    {
        try
        {
            _logger.LogInformation(
                $"Acquiring Bank - Processing payment to {paymentTransactionRequest.Details.Merchant} with value {paymentTransactionRequest.Details.Amount}");
            var transactionResponse = await _paymentService.ProcessTransaction(paymentTransactionRequest);
            return Created("api/payment/v1", transactionResponse);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                $"Ops, something went wrong when processing payment to {paymentTransactionRequest.Details.Merchant} with value of {paymentTransactionRequest.Details.Amount}",
                exception);
            return BadRequest(exception);
        }
    }
}