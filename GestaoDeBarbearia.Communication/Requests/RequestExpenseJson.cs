using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestExpenseJson
{
    /// <summary>
    /// Data de vencimento da despesa.
    /// </summary>
    [Required(ErrorMessage = "É obrigatório informar a data de vencimento da despesa.")]
    [DataType(DataType.DateTime, ErrorMessage = "Data de vencimento inválida")]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Data de pagamento da despesa.
    /// </summary>

    [DataType(DataType.DateTime, ErrorMessage = "Data hora de pagamento inválida")]
    public DateTime? PaymentDate { get; set; } = null;

    /// <summary>
    /// Valor da despesa
    /// </summary>

    [Required(ErrorMessage = "É obrigatório informar o valor total da despesa")]
    [Range(0.1, double.MaxValue, ErrorMessage = "O valor da despesa deve ser maior que zero")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Valor total pago da despesa até o momento
    /// </summary>
    /// 
    [Required(ErrorMessage = "É obrigatório informar o valor pago até o momento (Se nada for pago, enviar como zero)")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor total pago da despesa inválido")]
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Nome do fornecedor, empresa ou pessoa que recebeu o pagamento.
    /// </summary>
    public string? Supplier { get; set; } = null;

    /// <summary>
    /// Observações ou detalhes adicionais relevantes sobre a despesa.<br/>
    /// Pode ser salvo como nulo caso não haja informações adicionais.
    /// </summary>
    public string? Notes { get; set; } = null;

    /// <summary>
    /// Indica se é um gasto recorrente, eventual, anual, parcelado, etc.
    /// </summary>
    /// 
    [Required(ErrorMessage = "É obrigatório informar qual é o tipo de gasto (Recorrente ou Único)")]
    [EnumDataType(typeof(Recurrence), ErrorMessage = "Valor inválido")]
    public Recurrence Recurrence { get; set; }

    /// <summary>
    /// Número total de parcelas caso a despesa seja parcelada.<br/>
    /// Pode ser salvo como nulo caso não seja uma despesa parcelada.
    /// </summary>
    [Range(1, long.MaxValue, ErrorMessage = "Numero total de parcelas inválido")]
    public long? TotalInstallments { get; set; } = null;

    /// <summary>
    /// Número total de parcelas pagas até o momento.<br/>
    /// Pode ser salvo como nulo caso não seja uma despesa parcelada.
    /// </summary>
    [Range(0, long.MaxValue, ErrorMessage = "Numero total de parcelas pagas inválido")]
    public long? PaidInstallments { get; set; } = null;
}
