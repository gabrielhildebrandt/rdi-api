using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RDI.API;
using RDI.API.Requests;
using RDI.API.Responses;
using RDI.Application.Helpers;
using RDI.IntegrationsTests.Config;
using Xunit;

namespace RDI.IntegrationsTests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class CardControllerTests
    {
        public CardControllerTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture ?? throw new ArgumentNullException(nameof(testsFixture));
        }

        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        #region Create

        [Fact]
        public async Task Create_ShouldReturnTheNewCard()
        {
            // Arrange
            const string url = "/v1/cards";

            const int customerId = 1;
            const long cardNumber = 1234123412341234;
            const int cvv = 12345;

            var request = new CardRequest
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();
            var cardResponse = JsonConvert.DeserializeObject<CardResponse>(responseContentString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().Be($"/v1/cards/{cardResponse.CardId}");

            cardResponse.CreationDate.Should().BeBefore(DateTime.UtcNow);
            cardResponse.CreationDate.Should().BeAfter(DateTime.UtcNow.AddSeconds(-1).Date);
            cardResponse.Token.Should().Be(CardHelper.GenerateToken(cardNumber, cvv));
            cardResponse.CardId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            const string url = "/v1/cards";

            const int customerId = 1;
            const long cardNumber = 1234123412341234;
            const int cvv = 12345;

            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(1, 12341234123412341, 12345)]
        [InlineData(1, 1234123412341234, 123456)]
        public async Task Create_ShouldReturnBadRequest_WhenPassWrongParameters(int customerId, long cardNumber, int cvv)
        {
            // Arrange
            const string url = "/v1/cards";

            var request = new CardRequest
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region ValidateToken

        [Fact]
        public async Task ValidateToken_ShouldReturnValidToken_WhenPassRightParameters()
        {
            // Arrange
            const int customerId = 1;
            const long cardNumber = 1234123412341234;
            const int cvv = 12345;

            var card = await CreateCardAsync(customerId, cardNumber, cvv);

            var url = $"/v1/cards/{card.CardId}";

            var request = new ValidateTokenRequest
            {
                CustomerId = customerId,
                Token = card.Token,
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();
            var validateTokenResponse = JsonConvert.DeserializeObject<ValidateTokenResponse>(responseContentString);

            // Assert
            validateTokenResponse.Validated.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var cardId = Guid.NewGuid();

            var url = $"/v1/cards/{cardId}";

            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task<CardResponse> CreateCardAsync(int customerId, long cardNumber, int cvv)
        {
            const string url = "/v1/cards";

            var request = new CardRequest
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _testsFixture.Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CardResponse>(responseContentString);
        }

        [Theory]
        [InlineData(1, "1327de2a-32d9-407d-b514-6cf3add31820", 123456, "4bef8009-6df1-4d32-8841-da286de27e6a")]
        public async Task ValidateToken_ShouldReturnBadRequest_WhenPassWrongParameters(int customerId, string cardId, int cvv, string strToken)
        {
            // Arrange
            var url = $"/v1/cards/{cardId}";

            var request = new ValidateTokenRequest
            {
                CustomerId = customerId,
                Token = Guid.Parse(strToken),
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnInvalid_WhenCustomerIdIsIncorrect()
        {
            // Arrange
            const int customerId = 1;
            const long cardNumber = 1234123412341234;
            const int cvv = 12345;

            var card = await CreateCardAsync(customerId, cardNumber, cvv);

            var url = $"/v1/cards/{card.CardId}";

            var request = new ValidateTokenRequest
            {
                CustomerId = 2,
                Token = card.Token,
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();
            var validateTokenResponse = JsonConvert.DeserializeObject<ValidateTokenResponse>(responseContentString);

            // Assert
            validateTokenResponse.Validated.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnInvalid_WhenTokenIsIncorrect()
        {
            // Arrange
            const int customerId = 1;
            const long cardNumber = 1234123412341234;
            const int cvv = 12345;

            var card = await CreateCardAsync(customerId, cardNumber, cvv);

            var url = $"/v1/cards/{card.CardId}";

            var request = new ValidateTokenRequest
            {
                CustomerId = customerId,
                Token = Guid.NewGuid(),
                CVV = cvv
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();
            var validateTokenResponse = JsonConvert.DeserializeObject<ValidateTokenResponse>(responseContentString);

            // Assert
            validateTokenResponse.Validated.Should().BeFalse();
        }

        #endregion
    }
}