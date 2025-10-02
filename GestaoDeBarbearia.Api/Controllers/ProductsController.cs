using GestaoDeBarbearia.Application.UseCases.Products;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// Método para criar um novo produto no sistema.<br/>
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromServices] CreateProductUseCase useCase, [FromBody] RequestProductJson request)
    {
        await useCase.Execute(request);

        return Created(string.Empty, new { });
    }

    /// <summary>
    /// Método para buscar todos os produtos com paginação.<br/>
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="paginationParams"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseProductsJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchProducts([FromServices] FetchProductsUseCase useCase, [FromQuery] RequestProductsPaginationParamsJson paginationParams)
    {
        var result = await useCase.Execute(paginationParams);

        return Ok(result);
    }

    /// <summary>
    /// Método para buscar um único produto pelo seu Id.<br/>
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType<ResponseProductJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById([FromServices] GetProductByIdUseCase useCase, [FromRoute] long id)
    {
        var result = await useCase.Execute(id);

        return Ok(result);
    }

    /// <summary>
    /// Método para atualizar um produto existente pelo seu Id.<br/>
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProduct([FromServices] UpdateProductUseCase useCase, [FromBody] RequestProductJson request, [FromRoute] long id)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
