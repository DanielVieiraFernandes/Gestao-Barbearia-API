using GestaoDeBarbearia.Application.UseCases;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ResponseScheduledServiceJson>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] ScheduleServiceUseCase useCase,
        [FromBody] RequestScheduleServiceJson request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
