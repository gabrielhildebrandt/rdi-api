using System;
using FluentAssertions;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;
using Xunit;

namespace RDI.UnitTests
{
    public class CardTests
    {
        [Fact]
        public void Constructor_ShouldInstantiateCard()
        {
            // Arrange
            const int customerId = 1;
            const long number = 1234123412341234;
            var token = Guid.NewGuid();

            // Act
            var card = new Card(customerId, number, token);

            // Assert
            card.Id.Should().NotBeEmpty();
            card.CreationDate.Should().BeBefore(DateTime.UtcNow);
            card.CreationDate.Should().BeAfter(DateTime.UtcNow.AddSeconds(-1).Date);
            card.CustomerId.Should().Be(customerId);
            card.Number.Should().Be(number);
            card.Token.Should().Be(token);
        }

        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenNumberLengthIsHigherThan16Characters()
        {
            // Arrange
            const int customerId = 1;
            const long numberWith16Characters = 123412341234123412;
            var token = Guid.NewGuid();
            const string exceptionMessage = "Number must be smaller than 16 characters.";

            // Act
            Action action = () => new Card(customerId, numberWith16Characters, token);

            // Assert
            action.Should().Throw<DomainException>().WithMessage(exceptionMessage);
        }
    }
}