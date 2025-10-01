using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseProductJson
{
    public Product product { get; set; } = default!;
}
