using ERP.TN.Application.DTOs;

namespace ERP.TN.Application.Services;

public interface IPayrollService
{
    Task<List<PayrollRunDto>> GetRunsAsync();
    Task<PayrollRunDto> CreateRunAsync(CreatePayrollRunRequest request);
}

public class PayrollService : IPayrollService
{
    private readonly List<PayrollRunDto> _runs = new();

    public Task<List<PayrollRunDto>> GetRunsAsync()
    {
        return Task.FromResult(_runs);
    }

    public Task<PayrollRunDto> CreateRunAsync(CreatePayrollRunRequest request)
    {
        var run = new PayrollRunDto
        {
            Id = Guid.NewGuid(),
            Period = request.Period,
            EmployeeCount = request.EmployeeIds.Count,
            GrossTotal = 15000m,
            NetTotal = 11800m,
            Status = "validated"
        };

        _runs.Add(run);
        return Task.FromResult(run);
    }
}
