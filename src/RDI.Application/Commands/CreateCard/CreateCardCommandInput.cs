using RDI.Domain.Kernel;

namespace RDI.Application.Commands.CreateCard
{
    public class CreateCardCommandInput : CommandInput<CreateCardCommandResult>
    {
        public CreateCardCommandInput(int customerId, long cardNumber, int cvv)
        {
            CustomerId = customerId;
            CardNumber = cardNumber;
            CVV = cvv;
        }
        
        public int CustomerId { get; }

        public long CardNumber { get; }

        public int CVV { get; }
    }
}