using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrustyHands.Platform.Professionals.API.Features.Create.Events;
using TrustyHands.Platform.Professionals.API.Shared.Infrastructure.Persistence;
using TrustyHands.Platform.Professionals.API.Shared.Models;
using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Features.Create;

public static class CreateProfessional
{
    public record Command(string Name, string Email, Specialty Specialty) : IRequest<Result>;

    public record Result(Guid Id, string Message);

    public class Handler(ApplicationDbContext _context,
        DaprClient _daprClient,
        ILogger<Handler> _logger) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // Check for duplicate email
            var exists = await _context.Professionals
                .AnyAsync(p => p.Email == request.Email, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"Professional with email {request.Email} already exists");
            }

            var professional = new Professional
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Specialty = request.Specialty
            };

            _context.Professionals.Add(professional);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Professional created with ID {ProfessionalId}", professional.Id);

            // Publish event via Dapr Pub/Sub
            var @event = new ProfessionalRegisteredEvent(
                professional.Id,
                professional.Name,
                professional.Email,
                professional.Specialty,
                professional.CreatedAt
            );

            await _daprClient.PublishEventAsync(
                "pubsub",
                "professional-registered",
                @event,
                cancellationToken);

            _logger.LogInformation("Published ProfessionalRegistered event for ID: {ProfessionalId}", professional.Id);

            return new Result(professional.Id, "Professional created successfully");
        }
    }
}
