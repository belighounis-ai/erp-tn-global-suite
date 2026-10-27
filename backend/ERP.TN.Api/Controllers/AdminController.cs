using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using ERP.TN.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    [HttpGet("status")]
    [Authorize(Policy = "AdminOnly")]
    public ActionResult<object> Status()
    {
        return Ok(new { status = "ok", environment = "production", mode = "client-server" });
    }
}
