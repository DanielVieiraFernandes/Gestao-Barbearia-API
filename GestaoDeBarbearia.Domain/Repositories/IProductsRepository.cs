using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination.Products;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IProductsRepository
{
    Task Create(Product product);
    Task Update(Product product);
    Task<Product?> FindById(long id);
    Task<List<Product>> GetAll(RequestProductsPaginationParamsJson? paginationParams = null);
}
