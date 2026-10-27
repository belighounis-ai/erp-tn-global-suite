using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> GetInventory()
    {
        var result = await _inventoryService.GetInventoryAsync();
        return Ok(result);
    }

    [HttpPost("stock")]
    [Authorize(Policy = "OperationsAccess")]
    public async Task<IActionResult> AddStock([FromBody] AddStockRequest request)
    {
        var result = await _inventoryService.AddStockAsync(request);
        return Ok(result);
    }
}
