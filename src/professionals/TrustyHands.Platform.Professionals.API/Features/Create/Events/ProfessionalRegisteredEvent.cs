using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Features.Create.Events;

public record ProfessionalRegisteredEvent(
    Guid ProfessionalId,
    string Name,
    string Email,
    Specialty Speciality,
    DateTimeOffset RegisteredAt);
