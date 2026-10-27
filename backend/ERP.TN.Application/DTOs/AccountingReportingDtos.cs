namespace ERP.TN.Application.DTOs;

public class JournalCreateRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
}

public class JournalDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
}

public class ReportingSummaryDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetResult { get; set; }
    public decimal TotalTvaCollected { get; set; }
    public decimal TotalTvaDeductible { get; set; }
    public decimal NetTva { get; set; }
}

public class TvaSummaryDto
{
    public string Code { get; set; } = "STD";
    public string Label { get; set; } = "TVA standard";
    public decimal Rate { get; set; }
    public decimal Collected { get; set; }
    public decimal Deductible { get; set; }
}
