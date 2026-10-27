namespace ERP.TN.Domain.Entities;

public class ChartOfAccount
{
    public Guid Id { get; set; }
    public string CountryCode { get; set; } = "TN";
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "asset";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Journal
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "Achat";
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
}

public class Voucher
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string JournalCode { get; set; } = "Achat";
    public DateTime VoucherDate { get; set; } = DateTime.UtcNow;
    public string Reference { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "draft";
}

public class VoucherLine
{
    public Guid Id { get; set; }
    public Guid VoucherId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string DebitCredit { get; set; } = "DEBIT";
    public decimal Amount { get; set; }
}

public class BankAccount
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = "TND";
    public decimal Balance { get; set; }
}
