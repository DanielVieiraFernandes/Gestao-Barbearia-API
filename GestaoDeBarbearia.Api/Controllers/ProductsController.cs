using GestaoDeBarbearia.Application.UseCases.Products;
using GestaoDeBarbearia.Communication.Requests;
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
    public async Task<IActionResult> CreateProduct([FromServices] CreateProductUseCase useCase, [FromBody] RequestCreateProductJson request)
    {
        await useCase.Execute(request);

        return Created(string.Empty, new { });
    }
}
