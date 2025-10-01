namespace GestaoDeBarbearia.Domain.Pagination;
public class RequestProductsPaginationParamsJson : RequestPaginationParamsJson
{
    public OrderByProductColumn OrderByColumn { get; set; } = OrderByProductColumn.CreatedAt;
}
