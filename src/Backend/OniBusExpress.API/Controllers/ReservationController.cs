using Microsoft.AspNetCore.Mvc;
using OniBusExpress.Application.UseCases.Reservation.Cancel;
using OniBusExpress.Application.UseCases.Reservation.GetById;
using OniBusExpress.Application.UseCases.Reservation.Register;
using OniBusExpress.Communication.Requests;
using OniBusExpress.Communication.Responses;

namespace OniBusExpress.API.Controllers
{
    [Route("reservas")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredReservationJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterReservationUseCase useCase,
            [FromBody] RequestRegisterReservationJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseReservationJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetReservationByIdUseCase useCase,
            [FromRoute] string id)
        {
            var reservation = await useCase.Execute(id);

            return Ok(reservation);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(
            [FromServices] ICancelReservationUseCase useCase,
            [FromRoute] string id)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
