using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Products;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Products;
public class FetchProductsUseCase
{
    private IProductsRepository productRepository;
    public FetchProductsUseCase(IProductsRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ResponseProductsJson> Execute(RequestProductsPaginationParamsJson paginationParams)
    {
        var result = await productRepository.GetAll(paginationParams);

        return new ResponseProductsJson { products = result };
    }

}
