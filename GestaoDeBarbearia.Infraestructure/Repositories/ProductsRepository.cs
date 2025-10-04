using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Pagination.Products;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ProductsRepository : IProductsRepository
{

    private DBFunctions dbFunctions;

    public ProductsRepository(DBFunctions dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task Create(Product product)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateInsertQuery<Product>("barber_shop_products");

        await connection.ExecuteAsync(sql, product);
    }

    public async Task<Product?> FindById(long id)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string sql = "SELECT * FROM barber_shop_products WHERE id = @Id;";

        return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<List<Product>> GetAll(RequestProductsPaginationParamsJson? paginationParams = null)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        RequestProductsPaginationParamsJson paginationParamsJson = paginationParams ?? new();

        StringBuilder sql = new();

        sql.Append("SELECT * FROM barber_shop_products ");
        sql.Append($@"ORDER BY {paginationParamsJson.OrderByColumn.ToString().ToLower()} 
            {paginationParamsJson.OrderByDirection.GetEnumDescription()} ");

        var result = await connection.QueryAsync<Product>(sql.ToString(), paginationParams);


        if (result is null || !result.Any())
            return [];

        // Equivalente a result.ToList()
        return [.. result];

        
    }

    public async Task Update(Product product)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string updateQuery = DBFunctions.CreateUpdateQuery<Product>("barber_shop_products");

        await connection.ExecuteAsync(updateQuery, product);
    }
}
