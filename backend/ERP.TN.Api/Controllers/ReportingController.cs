using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("summary")]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> Summary()
    {
        return Ok(await _reportingService.GetSummaryAsync());
    }

    [HttpGet("tva")]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> GetTva()
    {
        return Ok(await _reportingService.GetTvaSummaryAsync());
    }
}
