using ERP.TN.Application.DTOs;
using ERP.TN.Application.Services;
using ERP.TN.Domain.Entities;
using System.Text;

namespace ERP.TN.Application.Services;

public interface ITnAccountingConfigService
{
    List<ChartOfAccountDto> GetDefaultChartOfAccounts();
    List<TaxRule> GetDefaultTaxRules();
}

public class TnAccountingConfigService : ITnAccountingConfigService
{
    public List<ChartOfAccountDto> GetDefaultChartOfAccounts()
    {
        return new List<ChartOfAccountDto>
        {
            new() { Id = Guid.NewGuid(), Code = "101", Name = "Capital social", Type = "equity", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "106", Name = "Réserves", Type = "equity", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "121", Name = "Emprunts", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "211", Name = "Banques", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "218", Name = "Caisse", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "301", Name = "Clients", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "401", Name = "Fournisseurs", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "445", Name = "TVA collectée", Type = "liability", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "4457", Name = "TVA déductible", Type = "asset", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "601", Name = "Achats de marchandises", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "607", Name = "Achats de matières premières", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "701", Name = "Ventes de marchandises", Type = "revenue", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "707", Name = "Charges de personnel", Type = "expense", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "708", Name = "Cotisations sociales", Type = "expense", CountryCode = "TN" }
        };
    }

    public List<TaxRule> GetDefaultTaxRules()
    {
        return new List<TaxRule>
        {
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "STD", Name = "TVA standard", Rate = 0.19m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "RED", Name = "TVA réduite", Rate = 0.07m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "EX", Name = "Exonéré", Rate = 0.00m }
        };
    }
}
