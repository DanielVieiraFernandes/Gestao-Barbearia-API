using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Products;
public class GetProductByIdUseCase
{
    private IProductsRepository productRepository;
    public GetProductByIdUseCase(IProductsRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ResponseProductJson> Execute(long id)
    {
        Product? product = await productRepository.FindById(id);

        if (product is null)
            throw new NotFoundException("Produto não encontrado");

        return new ResponseProductJson { product = product };
    }
}
