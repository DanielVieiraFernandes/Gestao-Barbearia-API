using GestaoDeBarbearia.Application.UseCases.Expenses;
using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    /// <summary>
    /// Este endpoint cria uma despesa no sistema
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseExpenseJson>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterExpense([FromServices] RegisterExpenseUseCase useCase,
        [FromBody] RequestExpenseJson request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var expense = await useCase.Execute(request);

        return Created(string.Empty, expense);
    }


    /// <summary>
    /// Este endpoint registra o pagamento de uma despesa no sistema
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    [ProducesResponseType<ResponseExpenseJson>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExpensePayment([FromServices] ExpensePaymentUseCase useCase, [FromBody] RequestExpensePaymentJson request, [FromRoute] long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await useCase.Execute(id, request);

        return Ok(result);
    }

}
