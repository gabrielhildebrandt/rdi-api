using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RDI.Infra;

namespace RDI.IntegrationsTests.Config
{
    public class ApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string TestingEnvironmentName = "Testing";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            DeleteDatabase(services);

            new DatabaseConfiguration(services).Handle();

            host.StartAsync();

            return host;
        }

        private static void DeleteDatabase(IServiceProvider services)
        {
            var context = services.GetRequiredService<Context>();
            context.Database.EnsureDeleted();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseStartup<TStartup>();
                    x.UseEnvironment(TestingEnvironmentName);
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services => { services.RemoveAll(typeof(IHostedService)); }).UseTestServer();
        }
    }
}