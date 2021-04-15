using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;
using RDI.Infra;
using RDI.Infra.Repositories;
using Environment = RDI.Domain.Kernel.Environment;
using IMediator = MediatR.IMediator;
using Mediator = MediatR.Mediator;

namespace RDI.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            _configuration = configurationBuilder.Build();
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public void ConfigureServices(IServiceCollection services)
        {
            var parameters = _configuration.AsEnumerable().ToList();
            var environmentName = _environment.EnvironmentName;
            var environment = new Environment(environmentName, parameters);
            services.AddSingleton<IEnvironment>(environment);

            services.AddDbContext<Context>();

            services.AddScoped<ICardRepository, CardRepository>();

            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<Domain.Kernel.IMediator, Domain.Kernel.Mediator>();

            const string applicationAssemblyName = "RDI.Application";
            AddMediatorHandlers(services, applicationAssemblyName);
            AddMediatorPipelineBehaviors(services, applicationAssemblyName);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RDI API",
                    Version = "v1"
                });
            });

            services.AddControllers();
        }

        private static IServiceCollection AddMediatorHandlers(IServiceCollection services, string applicationAssemblyName)
        {
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            var classTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in classTypes)
            {
                var handlerInterfaceNames = new List<string>
                {
                    typeof(IRequestHandler<,>).Name,
                    typeof(INotificationHandler<>).Name
                };

                var handlerTypes = type.ImplementedInterfaces
                    .Select(i => i.GetTypeInfo())
                    .Where(i => handlerInterfaceNames.Contains(i.Name));

                foreach (var handlerType in handlerTypes)
                    services.AddTransient(handlerType.AsType(), type.AsType());
            }

            return services;
        }

        private static IServiceCollection AddMediatorPipelineBehaviors(IServiceCollection services, string applicationAssemblyName)
        {
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastPipelineBehavior<,>));

            return services;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RDI API Endpoints"));

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}