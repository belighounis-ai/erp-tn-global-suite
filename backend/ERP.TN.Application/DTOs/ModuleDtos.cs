namespace ERP.TN.Application.DTOs;

public class ChartOfAccountDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TN";
}

public class VoucherLineDto
{
    public Guid Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string DebitCredit { get; set; } = "DEBIT";
    public decimal Amount { get; set; }
}

public class VoucherDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string JournalCode { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "draft";
    public List<VoucherLineDto> Lines { get; set; } = new();
}

public class CreateVoucherRequest
{
    public string Number { get; set; } = string.Empty;
    public string JournalCode { get; set; } = "Achat";
    public string Reference { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; } = DateTime.UtcNow;
    public List<CreateVoucherLineRequest> Lines { get; set; } = new();
}

public class CreateVoucherLineRequest
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string DebitCredit { get; set; } = "DEBIT";
    public decimal Amount { get; set; }
}

public class PayrollRunDto
{
    public Guid Id { get; set; }
    public string Period { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal GrossTotal { get; set; }
    public decimal NetTotal { get; set; }
    public string Status { get; set; } = "draft";
}

public class CreatePayrollRunRequest
{
    public string Period { get; set; } = string.Empty;
    public List<Guid> EmployeeIds { get; set; } = new();
}

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Warehouse { get; set; } = string.Empty;
}

public class AddStockRequest
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Warehouse { get; set; } = string.Empty;
}

public class PurchaseOrderDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "draft";
}

public class CreatePurchaseOrderRequest
{
    public string Number { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class SalesOrderDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "draft";
}

public class CreateSalesOrderRequest
{
    public string Number { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
