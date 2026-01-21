using AutoMapper;
using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Employees.DTOs;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Zaměstnanec s ID {request.Id} nebyl nalezen");

        return _mapper.Map<EmployeeDto>(employee);
    }
}
