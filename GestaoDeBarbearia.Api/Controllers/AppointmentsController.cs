using GestaoDeBarbearia.Application.UseCases.Appointments;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    /// <summary>
    /// Registra um novo agendamento com os dados fornecidos.<br/>
    /// Este endpoint cria um novo agendamento no sistema e retorna os dados de confirmação.
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
    /// Confirma um agendamento existente alterando seu status para "confirmado". <br/>
    /// Essa ação é geralmente executada por um funcionário para reconhecer o agendamento.
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
    /// Marca um agendamento como "concluído", <br/> indicando que o serviço foi realizado com sucesso.<br/>
    /// Esta é a atualização de status final para um serviço.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <param name="appointmentid"></param>
    /// <returns></returns>
    [HttpPut("{appointmentid:long}/completed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsCompleted([FromServices] MarkServiceAsCompletedUseCase useCase, [FromBody] RequestMarkServiceCompletedJson request, [FromRoute] long appointmentid)
    {
        await useCase.Execute(appointmentid, request);

        return NoContent();
    }

    /// <summary>
    /// Retorna uma lista paginada de todos os agendamentos.<br/>
    /// Este endpoint permite filtrar e visualizar todos os serviços agendados no sistema.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseAppointmentsJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchAll([FromServices] FetchAppointmentsUseCase useCase, [FromQuery] RequestAppointmentsPaginationParamsJson pagination)
    {
        var response = await useCase.Execute(pagination);

        return Ok(response);
    }

    /// <summary>
    /// Retorna um único agendamento pelo seu ID exclusivo.<br/>
    /// É usado para buscar detalhes específicos de um agendamento em particular.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType<ResponseAppointmentJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromServices] GetAppointmentByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

}
