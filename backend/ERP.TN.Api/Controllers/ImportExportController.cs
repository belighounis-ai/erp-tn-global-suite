using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.TN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportExportController : ControllerBase
{
    private readonly IImportExportService _service;

    public ImportExportController(IImportExportService service)
    {
        _service = service;
    }

    [HttpGet("template/{type}")]
    public async Task<IActionResult> GetTemplate(string type, [FromQuery] string format = "xlsx")
    {
        var template = await _service.GetTemplateAsync(type, format);
        var bytes = await _service.ExportAsync(type, format);

        var contentType = format.Equals("csv", StringComparison.OrdinalIgnoreCase)
            ? "text/csv"
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(bytes, contentType, $"{type}-template.{format}");
    }

    [HttpGet("export/{type}")]
    public async Task<IActionResult> Export(string type, [FromQuery] string format = "xlsx")
    {
        var bytes = await _service.ExportAsync(type, format);
        var contentType = format.Equals("csv", StringComparison.OrdinalIgnoreCase)
            ? "text/csv"
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(bytes, contentType, $"{type}.{format}");
    }

    [HttpPost("import/{type}")]
    public async Task<IActionResult> Import(string type, IFormFile file)
    {
        var result = await _service.ImportAsync(type, file);
        return Ok(result);
    }
}
