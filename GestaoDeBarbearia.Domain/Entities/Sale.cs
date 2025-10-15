namespace GestaoDeBarbearia.Domain.Entities;
public class Sale
{
    public long Id { get; set; }
    public DateTime SaleDate { get; set; }
    public long SaleTotal { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SaleDetails> Details { get; set; } = [];
}
