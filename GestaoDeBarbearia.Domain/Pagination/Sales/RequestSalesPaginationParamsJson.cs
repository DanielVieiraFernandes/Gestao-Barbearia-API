namespace GestaoDeBarbearia.Domain.Pagination.Sales;
public class RequestSalesPaginationParamsJson : RequestPaginationParamsJson
{
    public OrderBySalesColumn OrderByColumn { get; set; } = OrderBySalesColumn.SaleDate;
}
