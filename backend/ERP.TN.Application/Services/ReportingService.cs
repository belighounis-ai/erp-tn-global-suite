using ERP.TN.Application.DTOs;

namespace ERP.TN.Application.Services;

public interface IJournalService
{
    Task<List<JournalDto>> GetAllAsync();
    Task<JournalDto> CreateAsync(JournalCreateRequest request);
}

public class JournalService : IJournalService
{
    private readonly List<JournalDto> _journals = new()
    {
        new JournalDto { Id = Guid.NewGuid(), Code = "ACH", Name = "Achats", CountryCode = "TN" },
        new JournalDto { Id = Guid.NewGuid(), Code = "VEN", Name = "Ventes", CountryCode = "TN" },
        new JournalDto { Id = Guid.NewGuid(), Code = "GEN", Name = "Journal Général", CountryCode = "TN" },
        new JournalDto { Id = Guid.NewGuid(), Code = "BAN", Name = "Banque", CountryCode = "TN" }
    };

    public Task<List<JournalDto>> GetAllAsync()
    {
        return Task.FromResult(_journals);
    }

    public Task<JournalDto> CreateAsync(JournalCreateRequest request)
    {
        var journal = new JournalDto
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            CountryCode = request.CountryCode
        };

        _journals.Add(journal);
        return Task.FromResult(journal);
    }
}

public interface IReportingService
{
    Task<ReportingSummaryDto> GetSummaryAsync();
    Task<List<TvaSummaryDto>> GetTvaSummaryAsync();
}

public class ReportingService : IReportingService
{
    public Task<ReportingSummaryDto> GetSummaryAsync()
    {
        return Task.FromResult(new ReportingSummaryDto
        {
            TotalRevenue = 487500.00m,
            TotalExpenses = 321200.00m,
            NetResult = 166300.00m,
            TotalTvaCollected = 83000.00m,
            TotalTvaDeductible = 64000.00m,
            NetTva = 19000.00m
        });
    }

    public Task<List<TvaSummaryDto>> GetTvaSummaryAsync()
    {
        return Task.FromResult(new List<TvaSummaryDto>
        {
            new() { Code = "STD", Label = "TVA standard", Rate = 0.19m, Collected = 71000.00m, Deductible = 54000.00m },
            new() { Code = "RED", Label = "TVA réduite", Rate = 0.07m, Collected = 12000.00m, Deductible = 10000.00m },
            new() { Code = "EX", Label = "Exonéré", Rate = 0.00m, Collected = 0m, Deductible = 0m }
        });
    }
}
