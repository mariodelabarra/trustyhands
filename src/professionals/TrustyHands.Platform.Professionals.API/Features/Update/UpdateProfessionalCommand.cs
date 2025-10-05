using MediatR;
using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Features.Update;

public record UpdateProfessionalCommand(Guid Id,
    string Name,
    Specialty Specialty) : IRequest<UpdateProfessionalResult>;

public record UpdateProfessionalResult(Guid Id, string Message);

