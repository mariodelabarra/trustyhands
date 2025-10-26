using Platform.TrustyHands.Professionals.API.Shared.Models;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Update.Events;

public record ProfessionalUpdatedEvent(Guid ProfessionalId,
    Professional OldData,
    Professional NewData,
    DateTime UpdatedAt);

