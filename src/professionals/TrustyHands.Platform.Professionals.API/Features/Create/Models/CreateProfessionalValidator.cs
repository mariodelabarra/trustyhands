using FluentValidation;
using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Features.Create.Models;

public class CreateProfessionalValidator : AbstractValidator<CreateProfessionalCommand>
{
    public CreateProfessionalValidator()
    {
        RuleFor(create => create.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

        RuleFor(x => x.Specialty)
            .NotEqual(Specialty.None).WithMessage("Specialty must not be empty");
    }
}
