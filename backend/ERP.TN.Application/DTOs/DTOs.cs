using ERP.TN.Domain.Entities;

namespace ERP.TN.Application.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
    public string CurrencyCode { get; set; } = "TND";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class CompanyCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
    public string CurrencyCode { get; set; } = "TND";
    public string? TaxRegistrationNumber { get; set; }
}

public class DashboardDto
{
    public int TotalCompanies { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetResult { get; set; }
}
