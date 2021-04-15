using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RDI.Application.Helpers;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.ValidateCardToken
{
    public class ValidateCardTokenCommandHandler : ICommandHandler<ValidateCardTokenCommandInput, ValidateCardTokenCommandResult>
    {
        private readonly ICardRepository _cardRepository;

        public ValidateCardTokenCommandHandler(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        public async Task<ValidateCardTokenCommandResult> Handle(ValidateCardTokenCommandInput command, CancellationToken cancellationToken)
        {
            var card = await _cardRepository
                .Get(x => x.Id == command.CardId)
                .FirstOrDefaultAsync(cancellationToken);

            var creationDateRuleFailed = card.CreationDate.AddMinutes(30) < DateTime.UtcNow;
            var customerOwnerRuleFailed = card.CustomerId != command.CustomerId;
            var tokenRuleFailed = CardHelper.GenerateToken(card.Number, command.CVV) != command.Token;

            if (creationDateRuleFailed || customerOwnerRuleFailed || tokenRuleFailed)
                return new ValidateCardTokenCommandResult(false);

            Debug.WriteLine($"Card Number: {card.Number}.");

            return new ValidateCardTokenCommandResult(true);
        }
    }
}