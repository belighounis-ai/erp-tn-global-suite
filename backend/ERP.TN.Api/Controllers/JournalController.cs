using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IJournalService _journalService;

    public JournalController(IJournalService journalService)
    {
        _journalService = journalService;
    }

    [HttpGet]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _journalService.GetAllAsync());
    }

    [HttpPost]
    [Authorize(Policy = "FinanceAccess")]
    public async Task<IActionResult> Create([FromBody] JournalCreateRequest request)
    {
        var result = await _journalService.CreateAsync(request);
        return Ok(result);
    }
}
