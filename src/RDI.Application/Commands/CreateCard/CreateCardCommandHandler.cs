using System;
using System.Threading;
using System.Threading.Tasks;
using RDI.Application.Helpers;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.CreateCard
{
    public class CreateCardCommandHandler : ICommandHandler<CreateCardCommandInput, CreateCardCommandResult>
    {
        public CreateCardCommandHandler(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        private readonly ICardRepository _cardRepository;

        public async Task<CreateCardCommandResult> Handle(CreateCardCommandInput command, CancellationToken cancellationToken)
        {
            var customerId = command.CustomerId;
            var number = command.CardNumber;
            var cvv = command.CVV;
            var token = CardHelper.GenerateToken(number, cvv);

            var card = new Card(customerId, number, token);

            await _cardRepository.AddAsync(card, cancellationToken);
            await _cardRepository.UnitOfWork.CommitAsync(cancellationToken);

            return new CreateCardCommandResult(card.CreationDate, card.Token, card.Id);
        }
    }
}