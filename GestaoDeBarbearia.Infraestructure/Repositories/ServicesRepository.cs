using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ServicesRepository : IServicesRepository
{
    private readonly DatabaseQueryBuilder dbFunctions;
    public ServicesRepository(DatabaseQueryBuilder dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task<List<Service>> FindAll(List<long>? ids = null)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = "SELECT * FROM barber_shop_services WHERE id = ANY(@Ids)";

        var result = await connection.QueryAsync<Service>(sql, new { Ids = ids });

        if (result is null || !result.Any())
            return [];

        return [.. result];
    }
}
