using TrustyHands.Platform.Professionals.API.Shared.Models;

namespace TrustyHands.Platform.Professionals.API.Features.Update.Events;

public record ProfessionalUpdatedEvent(Guid ProfessionalId,
    Professional OldData,
    Professional NewData,
    DateTime UpdatedAt);

