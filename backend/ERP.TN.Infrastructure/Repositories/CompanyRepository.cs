using ERP.TN.Application.Interfaces;
using ERP.TN.Domain.Entities;
using ERP.TN.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.TN.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _dbContext;

    public CompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _dbContext.Companies.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Company company)
    {
        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
    }
}
