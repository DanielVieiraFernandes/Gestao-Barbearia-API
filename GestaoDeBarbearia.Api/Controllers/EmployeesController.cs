using GestaoDeBarbearia.Application.UseCases.Users.Employees;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    /// <summary>
    /// Este endpoint cria um funcionário na barbearia
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseCreatedUserJson>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] RegisterEmployeeUseCase useCase,
        [FromBody] RequestEmployeeJson request)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }
}
