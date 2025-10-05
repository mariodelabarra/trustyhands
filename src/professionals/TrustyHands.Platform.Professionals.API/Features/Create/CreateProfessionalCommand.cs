using Dapr.Client;
using MediatR;
using TrustyHands.Platform.Professionals.API.Shared.Infrastructure.Persistence;
using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Features.Create;

public record CreateProfessionalCommand(string Name, string Email, Specialty Specialty) : IRequest<CreateProfessionalResult>;

public record CreateProfessionalResult(Guid Id, string Message);
