namespace GestaoDeBarbearia.Domain.Entities;
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public decimal UnitCost { get; set; }
    public long Quantity { get; set; }
    public int MinimumStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
