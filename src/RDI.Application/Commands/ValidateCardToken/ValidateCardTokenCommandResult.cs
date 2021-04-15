using RDI.Domain.Kernel;

namespace RDI.Application.Commands.ValidateCardToken
{
    public class ValidateCardTokenCommandResult : CommandResult
    {
        public ValidateCardTokenCommandResult(bool validated)
        {
            Validated = validated;
        }

        public bool Validated { get; }
    }
}