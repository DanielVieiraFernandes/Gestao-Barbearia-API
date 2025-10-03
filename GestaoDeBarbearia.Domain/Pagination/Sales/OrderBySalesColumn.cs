using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Pagination.Sales;
public enum OrderBySalesColumn
{
    [Description("Ordenar pela data de criação")]
    SaleDate = 1,
    [Description("Ordenar pelo total da venda")]
    SaleTotal = 2,
}
