using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseProductsJson
{
    public List<Product> products { get; set; } = [];
}
