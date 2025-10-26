using Dapr.Client;
using MediatR;
using Platform.TrustyHands.Professionals.API.Shared.Infrastructure.Persistence;
using Platform.TrustyHands.Professionals.API.Shared.Models.Enums;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Create;

public record CreateProfessionalCommand(string Name, string Email, Specialty Specialty) : IRequest<CreateProfessionalResult>;

public record CreateProfessionalResult(Guid Id, string Message);
