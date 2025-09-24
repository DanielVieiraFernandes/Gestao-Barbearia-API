using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Domain.Pagination;
public class RequestPaginationParamsJson
{
    public OrderByDirection OrderBy { get; set; } = OrderByDirection.DESCENDING;

    [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Selecione um status válido")]
    public AppointmentStatus? Status { get; set; } = null;
}
