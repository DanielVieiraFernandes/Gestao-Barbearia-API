using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseSaleJson
{
    public Sale sale { get; set; } = default!;
}
