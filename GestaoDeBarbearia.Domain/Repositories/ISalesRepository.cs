using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination.Sales;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface ISalesRepository
{
    Task<Sale> Create(Sale sale, List<SaleDetails> saleDetails);
    Task<Sale> FindById(long id);
    Task<List<SaleDetails>> FindDetailsById(long saleId);
    Task<List<Sale>> FilterSaleAndDetailsByMonth(DateOnly month);
    Task<List<Sale>> GetAll(RequestSalesPaginationParamsJson? paginationParams = null);
}
