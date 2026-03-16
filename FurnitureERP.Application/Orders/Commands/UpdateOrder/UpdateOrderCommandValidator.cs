using FluentValidation;

namespace FurnitureERP.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.OrderId)
            .GreaterThan(0).WithMessage("Neplatné ID objednávky");

        RuleFor(o => o.CustomerName)
            .NotEmpty().WithMessage("Jméno zákazníka je povinné")
            .MaximumLength(200).WithMessage("Jméno zákazníka nesmí překročit 200 znaků");

        RuleFor(o => o.CustomerPhone)
            .NotEmpty().WithMessage("Telefon zákazníka je povinný")
            .Matches(@"^\+?[0-9\s\-()]{9,20}$").WithMessage("Neplatný formát telefonu");

        RuleFor(o => o.DeliveryAddress)
            .NotEmpty().WithMessage("Adresa dodání je povinná")
            .MaximumLength(500).WithMessage("Adresa dodání nesmí překročit 500 znaků");

        RuleFor(o => o.CustomerEmail)
            .EmailAddress().WithMessage("Neplatný formát emailu")
            .When(o => !string.IsNullOrWhiteSpace(o.CustomerEmail));

        RuleFor(o => o.ExpectedCompletionDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Datum dokončení nemůže být v minulosti")
            .When(o => o.ExpectedCompletionDate.HasValue);
    }
}
