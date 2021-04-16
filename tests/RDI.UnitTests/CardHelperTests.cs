using System;
using FluentAssertions;
using RDI.Application.Helpers;
using Xunit;

namespace RDI.UnitTests
{
    public class CardHelperTests
    {
        [Theory]
        [InlineData(1234123412341234, 123, "e5ecdfc8-68cc-9224-06e4-690fc4737a8d")]
        [InlineData(1234123412341234, 321, "50e8fd92-24d8-bac2-9b56-3cb6fa4078c3")]
        [InlineData(4321432143214321, 123, "2d40d489-3dc0-733b-18bb-ac10203034ab")]
        [InlineData(4321432143214321, 321, "568d4656-a507-f1aa-604f-f5e15593b003")]
        public void GenerateToken_ShouldGenerateAValidToken(long number, int cvv, string strExpectedToken)
        {
            // Arrange
            var expectedToken = Guid.Parse(strExpectedToken);

            // Act
            var token = CardHelper.GenerateToken(number, cvv);

            // Assert
            token.Should().Be(expectedToken);
        }
    }
}