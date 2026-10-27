namespace ERP.TN.Domain.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = "fr";
    public string CurrencyCode { get; set; } = "TND";
}

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
    public string CurrencyCode { get; set; } = "TND";
    public string? TaxRegistrationNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
}

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
}

public class Product
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal BuyingPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal StockAlertLevel { get; set; }
}

public class Account
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "asset";
    public string CountryCode { get; set; } = "TN";
}

public class TaxRule
{
    public Guid Id { get; set; }
    public string CountryCode { get; set; } = "TN";
    public string Code { get; set; } = "STD";
    public string Name { get; set; } = "TVA standard";
    public decimal Rate { get; set; } = 0.19m;
}

public class Employee
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal MonthlySalary { get; set; }
}
