using System;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.ValidateCardToken
{
    public class ValidateCardTokenCommandInput : CommandInput<ValidateCardTokenCommandResult>
    {
        public ValidateCardTokenCommandInput(int customerId, Guid cardId, Guid token, int cvv)
        {
            CustomerId = customerId;
            CardId = cardId;
            Token = token;
            CVV = cvv;
        }

        public int CustomerId { get; }

        public Guid CardId { get; }

        public Guid Token { get; }

        public int CVV { get; }
    }
}