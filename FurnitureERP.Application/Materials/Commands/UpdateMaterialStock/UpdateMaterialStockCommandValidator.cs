using FluentValidation;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterialStock;

public class UpdateMaterialStockCommandValidator : AbstractValidator<UpdateMaterialStockCommand>
{
    public UpdateMaterialStockCommandValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0).WithMessage("Identifikátor materiálu musí být větší než 0");

        RuleFor(m => m.Quantity)
            .NotEqual(0).WithMessage("Množství nesmí být rovno 0");
    }
}
