using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Pagination.Products;
public enum OrderByProductColumn
{
    [Description("Ordenar pela data de criação do produto")]
    CreatedAt = 1,
    [Description("Ordenar pela quantidade do produto")]
    Quantity = 2,
    [Description("Ordenar pelo estoque mínimo do produto")]
    MinimumStock = 3,
}
