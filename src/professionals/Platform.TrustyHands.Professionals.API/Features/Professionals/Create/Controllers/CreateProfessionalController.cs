using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Create.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProfessionalController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> Create([FromBody] CreateProfessionalCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Results.Created($"/api/professionals/{result.Id}", result);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()));
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        }
    }
}
