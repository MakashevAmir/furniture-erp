using FluentValidation;

namespace FurnitureERP.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("Jméno zaměstnance je povinné")
            .MaximumLength(100).WithMessage("Jméno nesmí překročit 100 znaků");

        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage("Příjmení zaměstnance je povinné")
            .MaximumLength(100).WithMessage("Příjmení nesmí překročit 100 znaků");

        RuleFor(e => e.Position)
            .NotEmpty().WithMessage("Pozice zaměstnance je povinná")
            .MaximumLength(100).WithMessage("Pozice nesmí překročit 100 znaků");

        RuleFor(e => e.HourlyRate)
            .GreaterThan(0).WithMessage("Hodinová sazba musí být větší než 0");
    }
}
