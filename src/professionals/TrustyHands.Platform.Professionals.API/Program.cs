
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrustyHands.Platform.Professionals.API.Features.Create;
using TrustyHands.Platform.Professionals.API.Features.Create.Models;
using TrustyHands.Platform.Professionals.API.Shared.Infrastructure.Behaviors;
using TrustyHands.Platform.Professionals.API.Shared.Infrastructure.Persistence;

namespace TrustyHands.Platform.Professionals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<CreateProfessionalValidator>();

            // Dapr: Add Dapr client
            builder.Services.AddDaprClient();

            builder.Services.AddControllers()
                .AddDapr(); // Dapr: enables Dapr pub/sub binding attributes

            // Dapr Sidekick (for local development)
            builder.Services.AddDaprSidekick(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCloudEvents(); // Dapr: Required for pub/sub

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapSubscribeHandler(); // Dapr: Required for pub/sub

            // Minimal API Endpoint
            app.MapPost("/api/professionals", async (
                CreateProfessional.Command command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await mediator.Send(command, cancellationToken);
                    return Results.Created($"/api/professionals/{result.Id}", result);
                }
                catch (ValidationException ex)
                {
                    return Results.ValidationProblem(ex.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()));
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("CreateProfessional")
            .WithOpenApi();

            app.Run();
        }
    }
}
