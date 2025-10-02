using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Products;
public class UpdateProductUseCase
{
    private IProductRepository productRepository;
    public UpdateProductUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task Execute(long id, RequestProductJson request)
    {
        var product = await productRepository.FindById(id);

        if (product is null)
            throw new NotFoundException("Produto não encontrado");

        product.Name = request.Name;
        product.SalePrice = request.SalePrice;
        product.UnitCost = request.UnitCost;
        product.Quantity = request.Quantity;
        product.MinimumStock = request.MinimumStock;
        product.UpdatedAt = DateTime.UtcNow;

        await productRepository.Update(product);
    }
}
