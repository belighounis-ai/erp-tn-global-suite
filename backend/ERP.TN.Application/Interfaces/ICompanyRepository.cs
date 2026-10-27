using ERP.TN.Application.Interfaces;
using ERP.TN.Domain.Entities;

namespace ERP.TN.Application.Interfaces;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
    Task AddAsync(Company company);
}
