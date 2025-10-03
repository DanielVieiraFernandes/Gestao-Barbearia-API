namespace GestaoDeBarbearia.Domain.Pagination.Products;
public class RequestProductsPaginationParamsJson : RequestPaginationParamsJson
{
    public OrderByProductColumn OrderByColumn { get; set; } = OrderByProductColumn.CreatedAt;
}
