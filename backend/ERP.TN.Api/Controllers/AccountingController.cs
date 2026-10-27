using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountingController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public AccountingController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var result = await _accountingService.GetAccountsAsync();
        return Ok(result);
    }

    [HttpGet("vouchers")]
    public async Task<IActionResult> GetVouchers()
    {
        var result = await _accountingService.GetVouchersAsync();
        return Ok(result);
    }

    [HttpPost("vouchers")]
    public async Task<IActionResult> PostVoucher([FromBody] ERP.TN.Application.DTOs.CreateVoucherRequest request)
    {
        var result = await _accountingService.PostVoucherAsync(request);
        return Ok(result);
    }
}
