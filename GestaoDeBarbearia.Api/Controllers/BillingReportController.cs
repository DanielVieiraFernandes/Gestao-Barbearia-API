using GestaoDeBarbearia.Application.UseCases.Billing.Reports.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GestaoDeBarbearia.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BillingReportController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] GenerateBillingReportUseCase useCase, [FromQuery] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
    }
}