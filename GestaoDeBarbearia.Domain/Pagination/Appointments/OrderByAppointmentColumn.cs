using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Pagination.Appointments;
public enum OrderByAppointmentColumn
{
    [Description("Ordenar pela data de criação do agendamento")]
    CreatedAt = 1,

    [Description("Ordernar pela data do agendamento")]
    AppointmentDateTime = 2,

    [Description("Ordernar pela data do pagamento")]
    PaidAt = 3,
}
