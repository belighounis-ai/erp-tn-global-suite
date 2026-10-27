using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new { Name = "Admin", Description = "Administration" },
            new { Name = "Finance", Description = "Comptabilité et finance" },
            new { Name = "Achats", Description = "Achats et fournisseurs" },
            new { Name = "Ventes", Description = "Ventes et clients" },
            new { Name = "Logistique", Description = "Stock et logistique" }
        });
    }
}
