using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IProductRepository
{
    Task Create(Product product);
    Task Update(Product product);
    Task<Product?> FindById(long id);
    Task<List<Product>> GetAll(RequestProductsPaginationParamsJson paginationParams);
}
