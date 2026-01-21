using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Employees;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var employee = new Employee(
            request.FirstName,
            request.LastName,
            request.Position,
            request.HourlyRate
        );

        await _employeeRepository.AddAsync(employee, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}
