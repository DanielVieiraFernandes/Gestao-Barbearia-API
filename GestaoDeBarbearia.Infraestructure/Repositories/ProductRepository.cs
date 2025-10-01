using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ProductRepository : IProductRepository
{

    private DBFunctions dbFunctions;

    public ProductRepository(DBFunctions dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task Create(Product product)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string sql = dbFunctions.CreateInsertQuery<Product>("barber_shop_products");

        await connection.ExecuteAsync(sql, product);
    }

    public Task<Product?> FindById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> GetAll(RequestProductsPaginationParamsJson paginationParams)
    {
        throw new NotImplementedException();
    }

    public Task Update(Product product)
    {
        throw new NotImplementedException();
    }
}
