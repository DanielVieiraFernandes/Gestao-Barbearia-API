using Microsoft.Extensions.Configuration;
using Npgsql;

namespace GestaoDeBarbearia.Infraestructure.Utils;
public class DBFunctions
{
    private readonly string connectionString;
    public DBFunctions(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Connection")!;
    }
    public async Task<NpgsqlConnection> CreateNewConnection()
    {
        NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
