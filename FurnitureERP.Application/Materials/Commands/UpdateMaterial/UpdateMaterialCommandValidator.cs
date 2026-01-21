using FluentValidation;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterial;

public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
{
    public UpdateMaterialCommandValidator()
    {
        RuleFor(m => m.Id)
            .GreaterThan(0).WithMessage("Identifikátor materiálu musí být větší než 0");

        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Název materiálu je povinný")
            .MaximumLength(200).WithMessage("Název nesmí překročit 200 znaků");

        RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Popis materiálu je povinný");

        RuleFor(m => m.Category)
            .NotEmpty().WithMessage("Kategorie materiálu je povinná");

        RuleFor(m => m.Unit)
            .NotEmpty().WithMessage("Měrná jednotka je povinná")
            .MaximumLength(20).WithMessage("Měrná jednotka nesmí překročit 20 znaků");

        RuleFor(m => m.PricePerUnit)
            .GreaterThanOrEqualTo(0).WithMessage("Cena za jednotku nesmí být záporná");

        RuleFor(m => m.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("Minimální stav nesmí být záporný");

        RuleFor(m => m.Supplier)
            .NotEmpty().WithMessage("Dodavatel je povinný")
            .MaximumLength(200).WithMessage("Název dodavatele nesmí překročit 200 znaků");
    }
}
