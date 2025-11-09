using GestaoDeBarbearia.Application.UseCases.Billing.Reports.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GestaoDeBarbearia.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BillingReportController : ControllerBase
{
    /// <summary>
    /// Este endpoint devolve um relatório financeiro de faturamento da barbearia<br/>
    /// Calcula o imposto com base nos serviços prestados e nos produtos vendidos.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] GenerateBillingReportUseCase useCase,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
       )
    {

        // -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Se o filtro de data não for enviado
        // Então, o relatório será gerado com base no mês atual
        // -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        if (startDate is null || endDate is null)
        {
            DateTime dateNow = DateTime.Now;

            startDate = new DateTime(
                year: dateNow.Year,
                month: dateNow.Month,
                day: 1);

            endDate = new DateTime(year: dateNow.Year,
                month: dateNow.Month,
                day: DateTime.DaysInMonth(dateNow.Year, dateNow.Month));
        }

        byte[] file = await useCase.Execute(
            startDate: (DateTime)startDate,
            endDate: (DateTime)endDate);

        return File(file,
            MediaTypeNames.Application.Octet,
            $"Relatorio-Faturamento-{startDate}_a_{endDate}.xlsx");
    }
}