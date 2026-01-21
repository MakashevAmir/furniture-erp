using AutoMapper;
using FurnitureERP.Application.Employees.DTOs;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeesByPosition;

public class GetEmployeesByPositionQueryHandler : IRequestHandler<GetEmployeesByPositionQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeesByPositionQueryHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesByPositionQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Position))
            throw new ArgumentException("Pozice nesmí být prázdná", nameof(request.Position));

        var employees = _employeeRepository
            .GetEmployeesByPosition(request.Position)
            .ToList();

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}
