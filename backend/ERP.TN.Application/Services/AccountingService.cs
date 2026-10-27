using ERP.TN.Application.DTOs;

namespace ERP.TN.Application.Services;

public interface IAccountingService
{
    Task<List<ChartOfAccountDto>> GetAccountsAsync();
    Task<List<VoucherDto>> GetVouchersAsync();
    Task<VoucherDto> PostVoucherAsync(CreateVoucherRequest request);
}

public class AccountingService : IAccountingService
{
    private readonly List<ChartOfAccountDto> _accounts = new()
    {
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "101", Name = "Capital social", Type = "equity", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "211", Name = "Banques", Type = "asset", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "218", Name = "Caisse", Type = "asset", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "301", Name = "Clients", Type = "asset", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "401", Name = "Fournisseurs", Type = "liability", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "445", Name = "TVA collectée", Type = "liability", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "601", Name = "Achats", Type = "expense", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "707", Name = "Charges de personnel", Type = "expense", CountryCode = "TN" },
        new ChartOfAccountDto { Id = Guid.NewGuid(), Code = "701", Name = "Ventes", Type = "revenue", CountryCode = "TN" }
    };

    private readonly List<VoucherDto> _vouchers = new();

    public Task<List<ChartOfAccountDto>> GetAccountsAsync()
    {
        return Task.FromResult(_accounts);
    }

    public Task<List<VoucherDto>> GetVouchersAsync()
    {
        return Task.FromResult(_vouchers);
    }

    public Task<VoucherDto> PostVoucherAsync(CreateVoucherRequest request)
    {
        var voucher = new VoucherDto
        {
            Id = Guid.NewGuid(),
            Number = request.Number,
            JournalCode = request.JournalCode,
            Reference = request.Reference,
            VoucherDate = request.VoucherDate,
            TotalAmount = request.Lines.Sum(x => x.Amount),
            Status = "posted",
            Lines = request.Lines.Select(l => new VoucherLineDto
            {
                Id = Guid.NewGuid(),
                AccountCode = l.AccountCode,
                AccountName = l.AccountName,
                DebitCredit = l.DebitCredit,
                Amount = l.Amount
            }).ToList()
        };

        _vouchers.Add(voucher);
        return Task.FromResult(voucher);
    }
}
