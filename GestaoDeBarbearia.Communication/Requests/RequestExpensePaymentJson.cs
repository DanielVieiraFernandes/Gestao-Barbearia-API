using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestExpensePaymentJson
{
    [Required(ErrorMessage = "É obrigatório informar a data de pagamento")]
    [DataType(DataType.DateTime, ErrorMessage = "Data hora de pagamento inválida")]
    public DateTime PaymentDate { get; set; }

    [Required(ErrorMessage = "É obrigatório informar o valor do pagamento")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Valor do pagamento inválido")]
    public decimal Amount { get; set; }

    [StringLength(400, ErrorMessage = "Descrição muito longa")]
    public string? Notes { get; set; } = string.Empty;
}
