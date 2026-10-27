using ERP.TN.Application.DTOs;
using ERP.TN.Domain.Entities;

namespace ERP.TN.Application.Services;

public interface TnAccountingCatalogProvider
{
    List<ChartOfAccountDto> GetFullChart();
    List<TaxRule> GetTvaRules();
}

public class TnAccountingCatalogProviderImpl : TnAccountingCatalogProvider
{
    public List<ChartOfAccountDto> GetFullChart()
    {
        return new List<ChartOfAccountDto>
        {
            new() { Id = Guid.NewGuid(), Code = "101", Name = "Capital social", Type = "equity", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "106", Name = "Réserves", Type = "equity", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "121", Name = "Emprunts", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "201", Name = "Fournisseurs", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "205", Name = "Dettes fiscales et sociales", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "211", Name = "Banques", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "218", Name = "Caisse", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "301", Name = "Clients", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "325", Name = "Créances diverses", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "401", Name = "Fournisseurs d’exploitation", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "409", Name = "Fournisseurs non-réguliers", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "445", Name = "TVA collectée", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "4457", Name = "TVA déductible", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "512", Name = "Banques", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "601", Name = "Achats de marchandises", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "606", Name = "Achats de matières premières", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "607", Name = "Achats de matières et fournitures", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "621", Name = "Autres charges externes", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "641", Name = "Salaires et traitements", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "645", Name = "Cotisations sociales", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "681", Name = "Charges financières", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "700", Name = "Ventes de marchandises", Type = "revenue", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "701", Name = "Ventes de produits", Type = "revenue", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "707", Name = "Charges de personnel", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "708", Name = "Services extérieurs", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "751", Name = "Produits financiers", Type = "revenue", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "771", Name = "Autres produits exceptionnels", Type = "revenue", CountryCode = "TN" }
        };
    }

    public List<TaxRule> GetTvaRules()
    {
        return new List<TaxRule>
        {
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "STD", Name = "TVA standard", Rate = 0.19m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "RED", Name = "TVA réduite", Rate = 0.07m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "EX", Name = "TVA exonérée", Rate = 0.00m }
        };
    }
}
