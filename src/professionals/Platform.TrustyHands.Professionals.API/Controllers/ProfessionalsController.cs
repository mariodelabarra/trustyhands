using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Platform.TrustyHands.Professionals.API.Features.Professionals.Create;
using Platform.TrustyHands.Professionals.API.Features.Professionals.Update;

namespace Platform.TrustyHands.Professionals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionalsController : ApiControllerBase
    {
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IResult> Create([FromBody] CreateProfessionalCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Mediator.Send(command, cancellationToken);
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
                var result = await Mediator.Send(command, cancellationToken);
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
