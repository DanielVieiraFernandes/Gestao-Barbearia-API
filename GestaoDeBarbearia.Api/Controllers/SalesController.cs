using GestaoDeBarbearia.Application.UseCases.Sales;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    /// <summary>
    /// Método para registrar uma nova venda.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseSaleJson>(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterSale([FromBody] RequestRegisterSaleJson request, [FromServices] RegisterSaleUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}
