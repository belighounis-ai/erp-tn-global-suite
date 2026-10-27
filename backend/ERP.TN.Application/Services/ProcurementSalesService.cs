using ERP.TN.Application.DTOs;

namespace ERP.TN.Application.Services;

public interface IProcurementService
{
    Task<List<PurchaseOrderDto>> GetPurchaseOrdersAsync();
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request);
}

public class ProcurementService : IProcurementService
{
    private readonly List<PurchaseOrderDto> _purchaseOrders = new();

    public Task<List<PurchaseOrderDto>> GetPurchaseOrdersAsync()
    {
        return Task.FromResult(_purchaseOrders);
    }

    public Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request)
    {
        var order = new PurchaseOrderDto
        {
            Id = Guid.NewGuid(),
            Number = request.Number,
            SupplierName = request.SupplierName,
            TotalAmount = request.TotalAmount,
            Status = "validated"
        };

        _purchaseOrders.Add(order);
        return Task.FromResult(order);
    }
}

public interface ISalesService
{
    Task<List<SalesOrderDto>> GetSalesOrdersAsync();
    Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest request);
}

public class SalesService : ISalesService
{
    private readonly List<SalesOrderDto> _salesOrders = new();

    public Task<List<SalesOrderDto>> GetSalesOrdersAsync()
    {
        return Task.FromResult(_salesOrders);
    }

    public Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderRequest request)
    {
        var order = new SalesOrderDto
        {
            Id = Guid.NewGuid(),
            Number = request.Number,
            CustomerName = request.CustomerName,
            TotalAmount = request.TotalAmount,
            Status = "validated"
        };

        _salesOrders.Add(order);
        return Task.FromResult(order);
    }
}
