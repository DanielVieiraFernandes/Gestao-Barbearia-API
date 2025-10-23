namespace GestaoDeBarbearia.Domain.Entities;
public class SaleDetails
{
    public long Id { get; set; }
    public long SaleId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public decimal UnitSalePrice { get; set; }
}
