using System;
using RDI.Domain.Kernel;

namespace RDI.Domain.CardAggregateRoot
{
    public class Card : IAggregateRoot
    {
        public Card(int customerId, long number, Guid token)
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;

            if (number.ToString().Length > 16)
                throw new DomainException("Number must be smaller than 16 characters.");

            CustomerId = customerId;
            Number = number;
            Token = token;
        }

        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public int CustomerId { get; }

        public long Number { get; }

        public Guid Token { get; }
    }
}