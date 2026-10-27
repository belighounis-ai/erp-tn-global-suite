using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventory()
    {
        var result = await _inventoryService.GetInventoryAsync();
        return Ok(result);
    }

    [HttpPost("stock")]
    public async Task<IActionResult> AddStock([FromBody] AddStockRequest request)
    {
        var result = await _inventoryService.AddStockAsync(request);
        return Ok(result);
    }
}
