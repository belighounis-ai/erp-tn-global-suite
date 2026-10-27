using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchasesController : ControllerBase
{
    private readonly IProcurementService _procurementService;

    public PurchasesController(IProcurementService procurementService)
    {
        _procurementService = procurementService;
    }

    [HttpGet]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _procurementService.GetPurchaseOrdersAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> CreateOrder([FromBody] CreatePurchaseOrderRequest request)
    {
        var result = await _procurementService.CreatePurchaseOrderAsync(request);
        return Ok(result);
    }
}
