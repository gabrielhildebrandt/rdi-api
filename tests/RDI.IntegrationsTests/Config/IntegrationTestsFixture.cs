using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RDI.API;
using Xunit;

namespace RDI.IntegrationsTests.Config
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Startup>>
    {
    }

    public class IntegrationTestsFixture<TStartup> where TStartup : class
    {
        public IntegrationTestsFixture()
        {
            var webApplicationFactoryClientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost:5001"),
                HandleCookies = false,
                MaxAutomaticRedirections = 0
            };

            var factory = new ApiFactory<TStartup>();

            Client = factory.CreateClient(webApplicationFactoryClientOptions);
        }

        public HttpClient Client;
    }
}