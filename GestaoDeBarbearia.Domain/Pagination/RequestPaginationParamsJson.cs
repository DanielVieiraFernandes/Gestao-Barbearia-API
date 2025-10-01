using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Domain.Pagination;
public class RequestPaginationParamsJson
{
    
    public OrderByDirection OrderByDirection { get; set; } = OrderByDirection.DESCENDING;

    
}
