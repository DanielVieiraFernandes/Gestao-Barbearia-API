using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Entities;
public class Expense
{
    public long Id { get; set; }

    /// <summary>
    /// Data de vencimento da despesa.
    /// DateTime pois o Dapper não consegue mapear o tipo DateOnly
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Data de pagamento da despesa.
    /// </summary>
    public DateTime? PaymentDate { get; set; } = null;

    /// <summary>
    /// Valor da despesa
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Valor total pago da despesa até o momento
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Valor da parcela da despesa. Caso seja uma conta parcelada.<br/>
    /// Pode ser salvo como nulo caso não seja uma conta parcelada.
    /// </summary>
    public decimal? AmountOfInstallment { get; set; } = null;

    /// <summary>
    /// Se o gasto está pago, pendente ou agendado para futuro pagamento.
    /// </summary>
    public ExpenseStatus  Status { get; set; }

    /// <summary>
    /// Nome do fornecedor, empresa ou pessoa que recebeu o pagamento.
    /// </summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>
    /// Observações ou detalhes adicionais relevantes sobre a despesa.<br/>
    /// Pode ser salvo como nulo caso não haja informações adicionais.
    /// </summary>
    public string? Notes { get; set; } = null;

    /// <summary>
    /// Indica se é um gasto recorrente, eventual, anual, parcelado, etc.
    /// </summary>
    public Recurrence Recurrence { get; set; }

    /// <summary>
    /// Número total de parcelas e quantas já foram pagas, caso a despesa seja parcelada.<br/>
    /// Pode ser salvo como nulo caso não seja uma despesa parcelada.
    /// </summary>
    public long? TotalInstallments { get; set; } = null;
    /// <summary>
    /// Número total de parcelas pagas até o momento.<br/>
    /// Pode ser salvo como nulo caso não seja uma despesa parcelada.
    /// </summary>
    public long? PaidInstallments { get; set; } = null;
}
