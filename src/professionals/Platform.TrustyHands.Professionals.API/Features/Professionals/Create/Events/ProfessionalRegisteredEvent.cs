using Platform.TrustyHands.Professionals.API.Shared.Models.Enums;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Create.Events;

public record ProfessionalRegisteredEvent(
    Guid ProfessionalId,
    string Name,
    string Email,
    Specialty Speciality,
    DateTimeOffset RegisteredAt);
