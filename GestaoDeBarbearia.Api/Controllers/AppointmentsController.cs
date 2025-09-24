using GestaoDeBarbearia.Application.UseCases;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseScheduledServiceJson>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] ScheduleServiceUseCase useCase,
        [FromBody] RequestScheduleServiceJson request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{appointmentid:long}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ConfirmAppointment([FromServices] ConfirmScheduleUseCase useCase, [FromRoute] long appointmentid)
    {
        await useCase.Execute(appointmentid);

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="appointmentid"></param>
    /// <returns></returns>
    [HttpPatch("{appointmentid:long}/completed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsCompleted([FromServices] MarkServiceAsCompletedUseCase useCase, [FromRoute] long appointmentid)
    {
        await useCase.Execute(appointmentid);

        return NoContent();
    }

    /// <summary>
    /// Retorna os agendamentos feitos pelos clientes
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseAppointmentsJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchAll([FromServices] FetchAppointmentsUseCase useCase, [FromQuery] RequestPaginationParamsJson pagination)
    {
        var response = await useCase.Execute(pagination);

        return Ok(response);
    }


}
