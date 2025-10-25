using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Entities;
public class Appointment
{
    // Campos referentes à tabela barber_shop_appointments

    public long Id { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public DateTime AppointmentEndDateTime { get; set; }
    public long? ClientId { get; set; } = null;
    public string? ClientName { get; set; } = null;
    public string? ClientPhone { get; set; }
    public long? EmployeeId { get; set; }
    public AppointmentStatus Status { get; set; }
    public decimal ServicePrice { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Observations { get; set; } = string.Empty;

    /// <summary>
    /// Apenas para consultas com JOIN
    /// </summary>
    public List<Service> Services { get; set; } = [];
}
