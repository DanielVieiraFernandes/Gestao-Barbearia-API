using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestScheduleServiceJson
{
    [Required(ErrorMessage = "A data do agendamento deve ser informada")]
    [DataType(DataType.DateTime, ErrorMessage = "A data do agendamento deve ser válida")]
    [Range(typeof(DateTime), "1/1/2002", "1/1/2050", ErrorMessage = "A data do agendamento deve ser válida")]
    public DateTime AppointmentDateTime { get; set; }

    [Required(ErrorMessage = "É obrigatório informar os serviços desejados")]
    [MinLength(1, ErrorMessage = "Pelo menos um serviço deve ser selecionado")]
    public List<long> ServiceIds { get; set; } = [];

    [StringLength(200, MinimumLength = 1, ErrorMessage = "O nome do cliente deve ser válido")]
    public string? ClientName { get; set; }

    [StringLength(15, MinimumLength = 1, ErrorMessage = "O telefone do cliente deve ser válido")]
    public string? ClientPhone { get; set; }
    public long? ClientId { get; set; }
    public long EmployeeId { get; set; }

    [Required(ErrorMessage = "Método de pagamento não informado")]
    [Range(1, int.MaxValue, ErrorMessage = "O método de pagamento não foi informado")]
    public PaymentType PaymentType { get; set; }
}
