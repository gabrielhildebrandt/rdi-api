using FluentValidation;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.CreateCard
{
    public class CreateCardCommandInputValidator : CommandInputValidator<CreateCardCommandInput>
    {
        public CreateCardCommandInputValidator()
        {
            RuleFor(p => p.CardNumber.ToString())
                .NotNull()
                .Length(1, 16);

            RuleFor(p => p.CVV.ToString())
                .NotNull()
                .Length(1, 5);
        }
    }
}