using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Platform.TrustyHands.Professionals.API.Features.Professionals.Update.Events;
using Platform.TrustyHands.Professionals.API.Shared.Infrastructure.Persistence;
using Platform.TrustyHands.Professionals.API.Shared.Models;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Update
{
    public class UpdateProfessionalCommandHandler(
        ApplicationDbContext _context,
        DaprClient _daprClient,
        ILogger<UpdateProfessionalCommandHandler> _logger)
        : IRequestHandler<UpdateProfessionalCommand, UpdateProfessionalResult>
    {
        public async Task<UpdateProfessionalResult> Handle(UpdateProfessionalCommand request, CancellationToken cancellationToken)
        {
            var professional = await _context.Professionals
                .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (professional is null)
            {
                throw new KeyNotFoundException($"Professional with ID {request.Id} not found");
            }

            var hasChanges = professional.Name != request.Name ||
                professional.Specialty != request.Specialty;

            if (!hasChanges)
            {
                _logger.LogInformation("No information detected for Professional ID: {ProfessionalId}", professional.Id);
                return new UpdateProfessionalResult(professional.Id, "No changes detected");
            }

            var oldValue = new Professional { Email = professional.Email, Name = professional.Name, Specialty = professional.Specialty };

            professional.Name = request.Name;
            professional.Specialty = request.Specialty;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Professional updated with ID: {ProfessionalId}", professional.Id);

            var @event = new ProfessionalUpdatedEvent(
                professional.Id,
                oldValue,
                professional,
                DateTime.UtcNow);

            await _daprClient.PublishEventAsync(
                "pubsub",
                "professional-updated",
                @event,
                cancellationToken);

            _logger.LogInformation("Published ProfessionalUpdated event for ID: {ProfesionalId}", professional.Id);

            return new UpdateProfessionalResult(professional.Id, "Professional updated successfully");
        }
    }
}
