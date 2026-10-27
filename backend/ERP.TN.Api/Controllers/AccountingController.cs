using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using ERP.TN.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountingController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public AccountingController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet("accounts")]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> GetAccounts()
    {
        var result = await _accountingService.GetAccountsAsync();
        return Ok(result);
    }

    [HttpGet("vouchers")]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> GetVouchers()
    {
        var result = await _accountingService.GetVouchersAsync();
        return Ok(result);
    }

    [HttpPost("vouchers")]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> PostVoucher([FromBody] CreateVoucherRequest request)
    {
        var result = await _accountingService.PostVoucherAsync(request);
        return Ok(result);
    }
}
