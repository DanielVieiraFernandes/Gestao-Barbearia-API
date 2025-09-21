using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestScheduleServiceJson
{
    [Required(ErrorMessage = "A data do agendamento deve ser informada")]
    [DataType(DataType.DateTime, ErrorMessage = "A data do agendamento deve ser válida")]
    [Range(typeof(DateTime), "1/1/2002", "1/1/2050", ErrorMessage = "A data do agendamento deve ser válida")]
    public DateTime AppointmentDateTime { get; set; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "O nome do cliente deve ser válido")]
    public string? ClientName { get; set; }

    [StringLength(15, MinimumLength = 1, ErrorMessage = "O telefone do cliente deve ser válido")]
    public string? ClientPhone { get; set; }
    public long? ClientId { get; set; }
    public long? EmployeeId { get; set; }

    [Required(ErrorMessage = "O preço do serviço não foi informado")]
    [Range(1, long.MaxValue, ErrorMessage = "O preço do serviço deve ser maior que zero")]
    public long ServicePrice { get; set; }

    [Required(ErrorMessage = "Método de pagamento não informado")]
    [Range(1, int.MaxValue, ErrorMessage = "O método de pagamento não foi informado")]
    public PaymentType PaymentType { get; set; }
}
