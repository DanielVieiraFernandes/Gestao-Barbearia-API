using ClosedXML.Excel;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Billing.Reports.Excel;
public class GenerateBillingReportUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly ISchedulesRepository schedulesRepository;
    private readonly ISalesRepository salesRepository;
    private readonly IImpostosRepository impostosRepository;
    private readonly IExpensesRepository expensesRepository;

    public GenerateBillingReportUseCase(
        ISchedulesRepository schedulesRepository,
        ISalesRepository salesRepository,
        IImpostosRepository impostosRepository,
        IExpensesRepository expensesRepository
        )
    {
        this.schedulesRepository = schedulesRepository;
        this.salesRepository = salesRepository;
        this.impostosRepository = impostosRepository;
        this.expensesRepository = expensesRepository;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        List<Appointment> appointments = await schedulesRepository.FilterByMonth(month);
        List<Sale> sales = await salesRepository.FilterSaleAndDetailsByMonth(month);

        decimal totalBillingServices = (decimal)appointments.Sum(a => a.ServicePrice) / 100;
        decimal totalBillingSales = (decimal)sales.Sum(s => s.SaleTotal) / 100;

        ImpostoSimplesNacional impostoProduct = await impostosRepository.GetByType(FiscalType.Product, totalBillingSales);
        ImpostoSimplesNacional impostoService = await impostosRepository.GetByType(FiscalType.Service, totalBillingServices);

        DateTime startDate = new(year: month.Year, month: month.Month, day: 1);
        DateTime endDate = new(year: month.Year, month: month.Month, day: DateTime.DaysInMonth(month.Year, month.Month));

        decimal totalExpensesAmount = await expensesRepository.GetTotalAmount(startDate, endDate);

        decimal salesTaxesValue = (totalBillingServices * impostoService.Aliquota) / 100;

        decimal serviceTaxesValue = (totalBillingSales * impostoProduct.Aliquota) / 100;

        decimal clearAmount = ((totalBillingServices + totalBillingSales) - serviceTaxesValue - salesTaxesValue) - totalExpensesAmount;

        using (var workbook = new XLWorkbook())
        {
            // =================================================================
            // Aba 1: Relatório Gerencial
            // =================================================================
            var relatorioSheet = workbook.Worksheets.Add("Relatório Gerencial");

            relatorioSheet.Cell("A1").Value = "Resumo Geral do Mês";
            relatorioSheet.Range("A1:D1").Merge().Style.Font.Bold = true;
            relatorioSheet.Range("A1:D1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Faturamento
            relatorioSheet.Cell("A3").Value = "Faturamento Bruto Total";
            relatorioSheet.Cell("B3").Value = totalBillingServices;
            relatorioSheet.Cell("C3").Value = totalBillingSales;
            relatorioSheet.Cell("B3").Style.NumberFormat.Format = "\"R$\" #,##0.00";
            relatorioSheet.Cell("C3").Style.NumberFormat.Format = "\"R$\" #,##0.00";

            // Impostos sobre Serviços
            relatorioSheet.Cell("A4").Value = "Impostos sobre Serviços";
            relatorioSheet.Cell("B4").Value = salesTaxesValue;
            relatorioSheet.Cell("B4").Style.NumberFormat.Format = "\"R$\" #,##0.00";

            // Impostos sobre Vendas
            relatorioSheet.Cell("A5").Value = "Impostos sobre Vendas";
            relatorioSheet.Cell("C5").Value = serviceTaxesValue;
            relatorioSheet.Cell("C5").Style.NumberFormat.Format = "\"R$\" #,##0.00";

            // Valor das Despesas
            relatorioSheet.Cell("A7").Value = "Despesas Totais";
            relatorioSheet.Cell("B7").Value = totalExpensesAmount;
            relatorioSheet.Cell("B7").Style.NumberFormat.Format = "\"R$\" #,##0.00";

            // Lucro Líquido
            relatorioSheet.Cell("A8").Value = "Lucro Líquido do Mês";
            relatorioSheet.Cell("B8").Value = clearAmount;
            relatorioSheet.Cell("B8").Style.Font.Bold = true;
            relatorioSheet.Cell("B8").Style.NumberFormat.Format = "\"R$\" #,##0.00";

            // Ajuste de colunas
            relatorioSheet.Columns().AdjustToContents();


            // =================================================================
            // Aba 2: Detalhe de Serviços (Dados Brutos)
            // =================================================================
            var servicosSheet = workbook.Worksheets.Add("Detalhe de Serviços");

            var appointmentsPT = appointments.Select(a => new
            {
                DataHoraAgendamento = a.AppointmentDateTime,
                IdCliente = a.ClientId,
                IdFuncionario = a.EmployeeId,
                NomeCliente = a.ClientName,
                TelefoneCliente = a.ClientPhone,
                Status = a.Status.GetEnumDescription(),
                PrecoServico = a.ServicePrice,
                TipoPagamento = a.PaymentType.GetEnumDescription(),
                PagoEm = a.PaidAt,
                Observacoes = a.Observations
            }).ToList();

            // O método InsertTable é a forma mais fácil e poderosa de inserir uma lista!
            // Ele cria uma Tabela do Excel automaticamente, com cabeçalhos e filtros.
            servicosSheet.Cell("A1").InsertTable(appointmentsPT);
            servicosSheet.Columns().AdjustToContents();


            // =================================================================
            // Aba 3: Detalhe de Vendas de Produtos (Dados Brutos)
            // =================================================================
            var vendasSheet = workbook.Worksheets.Add("Detalhe de Vendas");

            var salesPT = sales.Select(s => new
            {
                s.Id,
                DataVenda = s.SaleDate,
                TotalVenda = s.SaleTotal,
                AtualizadoEm = s.UpdatedAt
            }).ToList();

            vendasSheet.Cell("A1").InsertTable(salesPT);
            vendasSheet.Columns().AdjustToContents();

            // ======================================================================
            // Aba 4: Detalhe dos Itens da Venda (SaleDetails) com referência à venda
            // ======================================================================
            var detalhesSheet = workbook.Worksheets.Add("Detalhe Itens de Venda");

            var detalhesPT = sales.SelectMany(s => s.Details.Select(d => new
            {
                IdVenda = s.Id,
                IdProduto = d.ProductId,
                Quantidade = d.Quantity,
                Subtotal = d.UnitSalePrice
            })).ToList();

            // ===========================================================================
            // Salvar o arquivo em um fluxo de memória para retornar na resposta da API
            // ===========================================================================
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray(); // Retorna o array de bytes do arquivo
        }


    }

}
