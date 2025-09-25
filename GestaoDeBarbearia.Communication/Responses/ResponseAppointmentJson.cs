using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseAppointmentJson
{
    public Appointment appointment { get; set; } = default!;
}
