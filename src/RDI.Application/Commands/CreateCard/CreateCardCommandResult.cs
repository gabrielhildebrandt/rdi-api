using System;
using RDI.Domain.Kernel;

namespace RDI.Application.Commands.CreateCard
{
    public class CreateCardCommandResult : CommandResult
    {
        public CreateCardCommandResult(DateTime creationDate, Guid token, Guid cardId)
        {
            CreationDate = creationDate;
            Token = token;
            CardId = cardId;
        }

        public DateTime CreationDate { get; }

        public Guid Token { get; }

        public Guid CardId { get; }
    }
}