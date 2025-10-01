using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Pagination;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;
using System.Text;

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

    public async Task<Product?> FindById(long id)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string sql = "SELECT * FROM barber_shop_products WHERE id = @Id;";

        return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<List<Product>> GetAll(RequestProductsPaginationParamsJson paginationParams)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("SELECT * FROM barber_shop_products ");
        sql.Append($@"ORDER BY {paginationParams.OrderByColumn.ToString().ToLower()} 
            {paginationParams.OrderByDirection.GetEnumDescription()} ");

        var result = await connection.QueryAsync<Product>(sql.ToString(), paginationParams);

        if (result is null || !result.Any())
            return [];

        // Equivalente a result.ToList()
        return [.. result];
    }

    public Task Update(Product product)
    {
        throw new NotImplementedException();
    }
}
