using FurnitureERP.Domain.Aggregates.Employees;

namespace FurnitureERP.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    IQueryable<Employee> GetActiveEmployees();

    IQueryable<Employee> GetEmployeesByPosition(string position);

    Task<Employee?> GetByFullNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);

    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);

    void Update(Employee employee);

    void Delete(Employee employee);
}
