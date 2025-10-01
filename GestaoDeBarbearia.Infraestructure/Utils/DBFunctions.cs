using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Reflection;

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

    public string CreateInsertQuery<T>(string tableName)
    {
        Type type = typeof(T);
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        List<string> columns = [];
        List<string> parameters = [];

        // Propriedades a serem excluídas da query
        List<string> excludeProps = ["Id", "CreatedAt", "UpdatedAt"];

        foreach (var prop in props)
        {
            // Ignorar propriedades que estão na lista de exclusão
            if (excludeProps.Contains(prop.Name))
                continue;

            columns.Add(prop.Name.ToLower());
            parameters.Add($"@{prop.Name}");
        }

        string insertSQL = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", parameters)});";

        return insertSQL;
    }
}
