using MediatR;
using Platform.TrustyHands.Professionals.API.Shared.Models.Enums;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Update;

public record UpdateProfessionalCommand(Guid Id,
    string Name,
    Specialty Specialty) : IRequest<UpdateProfessionalResult>;

public record UpdateProfessionalResult(Guid Id, string Message);

