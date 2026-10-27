using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _salesService.GetSalesOrdersAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateSalesOrderRequest request)
    {
        var result = await _salesService.CreateSalesOrderAsync(request);
        return Ok(result);
    }
}
