using System;
using System.Linq;
using System.Threading.Tasks;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Domain.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AcquiringBank.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
    {
        try
        {
            _logger.LogInformation($"Creating account to {createAccountRequest.ClientName}");
            var accountDetails = await _accountService.CreateAccountAsync(createAccountRequest.ClientName);

            return Created("/api/account/v1", accountDetails);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                $"Ops, something went wrong when creating client account: {createAccountRequest.ClientName}",
                exception);
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            _logger.LogInformation("Getting accounts paginated");
            var accountsPaginated = await _accountService.ListAccountsPaginatedAsync(pageNumber, pageSize);

            if (accountsPaginated.Items.Any())
                return Ok(accountsPaginated);

            return NotFound();
        }
        catch (Exception exception)
        {
            _logger.LogError("Ops, something went wrong when getting client accounts ", exception);
            return BadRequest(exception.Message);
        }
    }
}