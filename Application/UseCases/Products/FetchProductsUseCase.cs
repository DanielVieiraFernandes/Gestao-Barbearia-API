using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Products;
public class FetchProductsUseCase
{
    private IProductRepository productRepository;
    public FetchProductsUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ResponseProductsJson> Execute(RequestProductsPaginationParamsJson paginationParams)
    {
        var result = await productRepository.GetAll(paginationParams);

        return new ResponseProductsJson { products = result };
    }

}
