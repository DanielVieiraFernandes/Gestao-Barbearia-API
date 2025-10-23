namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseScheduledServiceJson
{
    public DateTime AppointmentDateTime { get; set; }
    public decimal ServicePrice { get; set; }
    public long? EmployeeId { get; set; }
}
