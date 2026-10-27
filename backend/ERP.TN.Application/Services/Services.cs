using ERP.TN.Application.DTOs;
using ERP.TN.Application.Interfaces;
using ERP.TN.Domain.Entities;

namespace ERP.TN.Application.Services;

public interface ICompanyService
{
    Task<List<CompanyDto>> GetAllAsync();
    Task<CompanyDto?> GetByIdAsync(Guid id);
    Task<CompanyDto> CreateAsync(CompanyCreateRequest request);
}

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}

public interface ICountryService
{
    Task<List<Country>> GetCountriesAsync();
}

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<CompanyDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();

        return companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            Name = c.Name,
            CountryCode = c.CountryCode,
            CurrencyCode = c.CurrencyCode,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company is null)
            return null;

        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            CountryCode = company.CountryCode,
            CurrencyCode = company.CurrencyCode,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt
        };
    }

    public async Task<CompanyDto> CreateAsync(CompanyCreateRequest request)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CountryCode = request.CountryCode,
            CurrencyCode = request.CurrencyCode,
            TaxRegistrationNumber = request.TaxRegistrationNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _companyRepository.AddAsync(company);

        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            CountryCode = company.CountryCode,
            CurrencyCode = company.CurrencyCode,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt
        };
    }
}

public class DashboardService : IDashboardService
{
    public Task<DashboardDto> GetDashboardAsync()
    {
        var dashboard = new DashboardDto
        {
            TotalCompanies = 12,
            TotalCustomers = 340,
            TotalSuppliers = 190,
            TotalProducts = 620,
            TotalRevenue = 487500.00m,
            TotalExpenses = 321200.00m,
            NetResult = 166300.00m
        };

        return Task.FromResult(dashboard);
    }
}

public class CountryService : ICountryService
{
    public Task<List<Country>> GetCountriesAsync()
    {
        var countries = new List<Country>
        {
            new Country { Id = Guid.NewGuid(), Code = "TN", Name = "Tunisie", CurrencyCode = "TND", Language = "fr" },
            new Country { Id = Guid.NewGuid(), Code = "FR", Name = "France", CurrencyCode = "EUR", Language = "fr" },
            new Country { Id = Guid.NewGuid(), Code = "MA", Name = "Maroc", CurrencyCode = "MAD", Language = "fr" },
            new Country { Id = Guid.NewGuid(), Code = "DZ", Name = "Algérie", CurrencyCode = "DZD", Language = "fr" }
        };

        return Task.FromResult(countries);
    }
}
