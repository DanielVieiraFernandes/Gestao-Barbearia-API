using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Domain.Pagination.Appointments;
public class RequestAppointmentsPaginationParamsJson : RequestPaginationParamsJson
{
    public OrderByAppointmentColumn OrderByColumn { get; set; } = OrderByAppointmentColumn.CreatedAt;

    [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Selecione um status válido")]
    public AppointmentStatus? Status { get; set; } = null;
}
