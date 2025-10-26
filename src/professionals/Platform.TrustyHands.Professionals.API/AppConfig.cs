using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Platform.TrustyHands.Professionals.API.Features.Professionals.Create.Models;
using Platform.TrustyHands.Professionals.API.Shared.Infrastructure.Behaviors;
using Platform.TrustyHands.Professionals.API.Shared.Infrastructure.Persistence;

namespace Platform.TrustyHands.Professionals.API
{
    public static class AppConfig
    {
        public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            RegisterConfiguration(services, configuration);
            RegisterServices(services);
            RegisterRepositories(services);
        }

        public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
        {

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateProfessionalValidator>();

            // Dapr: Add Dapr client
            services.AddDaprClient();

            // Dapr Sidekick (for local development)
            services.AddDaprSidekick(configuration);
        }

        public static void RegisterServices(IServiceCollection services)
        {

        }

        public static void RegisterRepositories(IServiceCollection services)
        {

        }
    }
}
