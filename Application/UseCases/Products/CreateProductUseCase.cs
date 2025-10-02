using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Products;
public class CreateProductUseCase
{
    private IProductRepository productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task Execute(RequestProductJson request)
    {
        Product product = new()
        {
            Name = request.Name,
            Quantity = request.Quantity,
            SalePrice = request.SalePrice,
            UnitCost = request.UnitCost,
            MinimumStock = request.MinimumStock
        };

        await productRepository.Create(product);
    }
}
