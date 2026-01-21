using FluentValidation;

namespace FurnitureERP.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Název výrobku je povinný")
            .MaximumLength(200).WithMessage("Název nesmí překročit 200 znaků");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Popis výrobku je povinný");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Kategorie výrobku je povinná");

        RuleFor(p => p.Article)
            .NotEmpty().WithMessage("Kód výrobku je povinný")
            .Matches(@"^[A-Z]{3}-\d{3}$").WithMessage("Kód musí být ve formátu ABC-123 (tři velká písmena, pomlčka, tři číslice)");

        RuleFor(p => p.BasePrice)
            .GreaterThan(0).WithMessage("Základní cena musí být větší než 0");
    }
}
