using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Platform.TrustyHands.Professionals.API.Features.Professionals.Update.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProfessionalController(IMediator _mediator) : ControllerBase
    {
        [HttpPut("{id:guid}")]
        public async Task<IResult> Update(Guid id, UpdateProfessionalCommand command, CancellationToken cancellationToken)
        {
            // Ensure the ID in the route matches the ID in the body
            if (id != command.Id)
            {
                return Results.BadRequest(new { error = "ID in route does not match ID in request body" });
            }

            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()));
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        }
    }
}
