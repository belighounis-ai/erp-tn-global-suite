using ERP.TN.Application.DTOs;
using ERP.TN.Domain.Entities;
using ERP.TN.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.TN.Application.Services;

public interface IAccountingSeedService
{
    Task SeedAsync();
}

public class AccountingSeedService : IAccountingSeedService
{
    private readonly AppDbContext _db;

    public AccountingSeedService(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.ChartOfAccounts.AnyAsync())
            return;

        var accounts = new List<ChartOfAccount>
        {
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "101", Name = "Capital social", Type = "equity" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "106", Name = "Réserves", Type = "equity" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "211", Name = "Banques", Type = "asset" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "218", Name = "Caisse", Type = "asset" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "301", Name = "Clients", Type = "asset" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "401", Name = "Fournisseurs", Type = "liability" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "445", Name = "TVA collectée", Type = "liability" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "4457", Name = "TVA déductible", Type = "asset" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "601", Name = "Achats", Type = "expense" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "701", Name = "Ventes", Type = "revenue" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "707", Name = "Charges de personnel", Type = "expense" },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "708", Name = "Cotisations sociales", Type = "expense" }
        };

        await _db.ChartOfAccounts.AddRangeAsync(accounts);

        var taxRules = new List<TaxRule>
        {
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "STD", Name = "TVA standard", Rate = 0.19m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "RED", Name = "TVA réduite", Rate = 0.07m },
            new() { Id = Guid.NewGuid(), CountryCode = "TN", Code = "EXEMPT", Name = "Exonéré", Rate = 0.00m }
        };

        await _db.TaxRules.AddRangeAsync(taxRules);

        var journals = new List<Journal>
        {
            new() { Id = Guid.NewGuid(), Code = "ACH", Name = "Achats", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "VEN", Name = "Ventes", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "GEN", Name = "Journal général", CountryCode = "TN" },
            new() { Id = Guid.NewGuid(), Code = "BAN", Name = "Banque", CountryCode = "TN" }
        };

        await _db.Journals.AddRangeAsync(journals);
        await _db.SaveChangesAsync();
    }
}
