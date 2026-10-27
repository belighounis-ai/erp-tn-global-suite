using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpGet]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> GetRuns()
    {
        var result = await _payrollService.GetRunsAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> CreateRun([FromBody] CreatePayrollRunRequest request)
    {
        var result = await _payrollService.CreateRunAsync(request);
        return Ok(result);
    }
}
