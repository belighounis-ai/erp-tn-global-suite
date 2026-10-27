using ERP.TN.Application.DTOs;

namespace ERP.TN.Application.Services;

public interface IInventoryService
{
    Task<List<InventoryItemDto>> GetInventoryAsync();
    Task<InventoryItemDto> AddStockAsync(AddStockRequest request);
}

public class InventoryService : IInventoryService
{
    private readonly List<InventoryItemDto> _stock = new()
    {
        new InventoryItemDto { Id = Guid.NewGuid(), ProductCode = "P001", ProductName = "Produit A", Quantity = 120, UnitPrice = 12.5m, Warehouse = "Entrepôt Centrale" },
        new InventoryItemDto { Id = Guid.NewGuid(), ProductCode = "P002", ProductName = "Produit B", Quantity = 85, UnitPrice = 20m, Warehouse = "Entrepôt Centrale" }
    };

    public Task<List<InventoryItemDto>> GetInventoryAsync()
    {
        return Task.FromResult(_stock);
    }

    public Task<InventoryItemDto> AddStockAsync(AddStockRequest request)
    {
        var item = new InventoryItemDto
        {
            Id = Guid.NewGuid(),
            ProductCode = request.ProductCode,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Warehouse = request.Warehouse
        };

        _stock.Add(item);
        return Task.FromResult(item);
    }
}
