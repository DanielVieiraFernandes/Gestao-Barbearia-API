namespace GestaoDeBarbearia.Domain.Entities;
public class Service
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long Price { get; set; }
    public bool Active { get; set; }
}
