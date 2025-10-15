using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Sales;
public class RegisterSaleUseCase
{
    private readonly ISalesRepository salesRepository;

    public RegisterSaleUseCase(ISalesRepository salesRepository)
    {
        this.salesRepository = salesRepository;
    }
    public async Task<ResponseSaleJson> Execute(RequestRegisterSaleJson request)
    {
        Sale sale = new()
        {
            SaleDate = request.SaleDate,
        };

        List<SaleDetails> saleDetails = request.DetailsProductSale.Select(d =>
        {
            return new SaleDetails
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
            };

        }).ToList();

        var result = await salesRepository.Create(sale, saleDetails);

        return new ResponseSaleJson
        {
            sale = result.Item1,
            saleDetails = result.Item2
        };
    }
}
