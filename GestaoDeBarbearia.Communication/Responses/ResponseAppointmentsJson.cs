using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseAppointmentsJson
{
    public List<Appointment> Appointments { get; set; } = [];
}
