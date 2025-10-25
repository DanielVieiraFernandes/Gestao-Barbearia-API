using GestaoDeBarbearia.Application.UseCases.Sales;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Sales;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;

/// <summary>
/// 
/// </summary>
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

    /// <summary>
    /// Método para recuperar vendas e detalhes das vendas com filtros.
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseSalesJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchSales([FromQuery] RequestSalesPaginationParamsJson pagination, [FromServices] FetchSalesUseCase useCase)
    {
        var result = await useCase.Execute(pagination);
        return Ok(result);
    }
}
