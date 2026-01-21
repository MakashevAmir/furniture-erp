using FluentValidation;

namespace FurnitureERP.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Identifikátor výrobku musí být větší než 0");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Název výrobku je povinný")
            .MaximumLength(200).WithMessage("Název nesmí překročit 200 znaků");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Popis výrobku je povinný");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Kategorie výrobku je povinná");

        RuleFor(p => p.BasePrice)
            .GreaterThan(0).WithMessage("Základní cena musí být větší než 0");
    }
}
