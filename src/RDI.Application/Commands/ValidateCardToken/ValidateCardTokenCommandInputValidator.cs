using System;
using System.Linq;
using FluentValidation;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.ValidateCardToken
{
    public class ValidateCardTokenCommandInputValidator : CommandInputValidator<ValidateCardTokenCommandInput>
    {
        public ValidateCardTokenCommandInputValidator(ICardRepository cardRepository)
        {
            if (cardRepository == null)
                throw new ArgumentNullException(nameof(cardRepository));

            RuleFor(p => p.CardId)
                .NotNull()
                .NotEmpty()
                .Custom((cardId, context) =>
                {
                    var exists = cardRepository
                        .Get(x => x.Id == cardId)
                        .Any();

                    if (!exists)
                        context.AddFailure($"Card not found with Id: {cardId}.");
                });

            RuleFor(p => p.CVV.ToString())
                .NotNull()
                .NotEmpty()
                .Length(1, 5);
        }
    }
}