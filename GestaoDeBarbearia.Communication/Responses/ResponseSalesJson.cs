using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseSalesJson
{
    public List<Sale> sales { get; set; } = [];
}
