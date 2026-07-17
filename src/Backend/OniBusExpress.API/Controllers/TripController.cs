using Microsoft.AspNetCore.Mvc;
using OniBusExpress.Application.UseCases.Trip.GetById;
using OniBusExpress.Application.UseCases.Trip.Search;
using OniBusExpress.Communication.Responses;

namespace OniBusExpress.API.Controllers
{
    [Route("viagens")]
    [ApiController]
    public class TripController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseTripsJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            [FromServices] ISearchTripsUseCase useCase,
            [FromQuery] string origin,
            [FromQuery] string destination,
            [FromQuery] DateOnly departureDate)
        {
            var result = await useCase.Execute(origin, destination, departureDate);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseTripDetailsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetTripByIdUseCase useCase,
            [FromRoute] Guid id)
        {
            var result = await useCase.Execute(id);

            return Ok(result);
        }
    }
}