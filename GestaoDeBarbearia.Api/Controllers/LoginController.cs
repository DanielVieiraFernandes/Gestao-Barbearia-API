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
public class LoginController : ControllerBase
{

    /// <summary>
    /// Este endpoint realiza o login de um funcionário
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("employee")]
    [ProducesResponseType<ResponseLoginJson>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResponseErrorJson>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] DoEmployeeLoginUseCase useCase, RequestLoginJson request)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }

}
