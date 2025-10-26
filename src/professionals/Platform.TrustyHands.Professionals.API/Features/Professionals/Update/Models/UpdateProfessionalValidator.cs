using FluentValidation;
using Platform.TrustyHands.Professionals.API.Features.Professionals.Update;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Update.Models
{
    public class UpdateProfessionalValidator : AbstractValidator<UpdateProfessionalCommand>
    {
        public UpdateProfessionalValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Professional ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 200 characters");
        }
    }
}
