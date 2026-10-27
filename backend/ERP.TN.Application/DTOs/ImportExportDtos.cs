namespace ERP.TN.Application.DTOs;

public class ImportExportTemplateDto
{
    public string Type { get; set; } = string.Empty;
    public string Format { get; set; } = "xlsx";
    public List<string> Headers { get; set; } = new();
    public List<Dictionary<string, string>> SampleRows { get; set; } = new();
}

public class ImportExportResultDto
{
    public string Type { get; set; } = string.Empty;
    public int TotalRows { get; set; }
    public int ImportedRows { get; set; }
    public int RejectedRows { get; set; }
    public List<string> Errors { get; set; } = new();
}
