using FurnitureERP.Domain.Aggregates.Employees;
using FurnitureERP.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public IQueryable<Employee> GetActiveEmployees()
    {
        return _context.Employees
            .AsNoTracking()
            .Where(e => e.IsActive);
    }

    public IQueryable<Employee> GetEmployeesByPosition(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
            return Enumerable.Empty<Employee>().AsQueryable();

        return _context.Employees
            .AsNoTracking()
            .Where(e => e.Position == position.Trim() && e.IsActive);
    }

    public async Task<Employee?> GetByFullNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            return null;

        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.FirstName == firstName.Trim() && e.LastName == lastName.Trim(), cancellationToken);
    }

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        await _context.Employees.AddAsync(employee, cancellationToken);
    }

    public void Update(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        _context.Employees.Update(employee);
    }

    public void Delete(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        _context.Employees.Remove(employee);
    }
}
