using Microsoft.AspNetCore.Mvc;
using OniBusExpress.Application.UseCases.Route.GetAll;
using OniBusExpress.Communication.Responses;

namespace OniBusExpress.API.Controllers
{
    [Route("rotas")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseRoutesJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromServices] IGetAllRoutesUseCase useCase)
        {
            var result = await useCase.Execute();

            return Ok(result);
        }
    }
}
