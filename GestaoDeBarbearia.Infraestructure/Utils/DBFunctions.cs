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

    public static string CreateInsertQuery<T>(string tableName)
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

        string insertSQL = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", parameters)})";

        return insertSQL;
    }

    public static string CreateUpdateQuery<T>(string tableName)
    {
        Type type = typeof(T);
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        List<string> setClauses = [];
        // Propriedades a serem excluídas da query
        List<string> excludeProps = ["Id", "CreatedAt", "UpdatedAt"];
        foreach (var prop in props)
        {
            // Ignorar propriedades que estão na lista de exclusão
            if (excludeProps.Contains(prop.Name))
                continue;
            setClauses.Add($"{prop.Name.ToLower()} = @{prop.Name}");
        }
        string updateSQL = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE id = @Id;";
        return updateSQL;
    }

    public static string CreateSelectAllQuery(string tableName)
    {
        return $"SELECT * FROM {tableName};";
    }

    public static string CreateSelectByIdQuery(string tableName, string? idColumn = null)
    {
        return $"SELECT * FROM {tableName} WHERE id = " + idColumn ?? "Id;";
    }

    public static string CreateDeleteQuery(string tableName)
    {
        return $"DELETE FROM {tableName} WHERE id = @Id;";
    }
}
