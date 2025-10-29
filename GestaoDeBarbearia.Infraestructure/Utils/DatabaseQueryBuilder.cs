using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Reflection;

namespace GestaoDeBarbearia.Infraestructure.Utils;
public class DatabaseQueryBuilder
{
    private readonly string connectionString;
    public DatabaseQueryBuilder(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Connection")!;
    }

    public async Task<NpgsqlConnection> CreateNewConnection()
    {
        NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        return connection;
    }

    public static string CreateInsertQuery<T>(string tableName, List<string>? excludeProps = null)
    {
        PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        Dictionary<string, string> columnsAndParams = [];

        // Propriedades a serem excluídas da query
        excludeProps ??= ["id", "createdat", "updatedat"];

        foreach (var prop in props)
        {
            // Ignorar propriedades que estão na lista de exclusão
            if (excludeProps.Contains(prop.Name.ToLower()))
                continue;

            columnsAndParams.Add(prop.Name.ToLower(), $"@{prop.Name}");
        }

        string insertSQL = $"INSERT INTO {tableName} ({string.Join(", ", columnsAndParams.Keys)}) VALUES ({string.Join(", ", columnsAndParams.Values)})";

        return insertSQL;
    }

    public static string CreateUpdateQuery<T>(string tableName, string conditionColumn = "Id", List<string>? excludeProps = null)
    {
        Type type = typeof(T);
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        List<string> setClauses = [];

        // Propriedades a serem excluídas da query
        excludeProps ??= ["id", "createdat"];

        foreach (var prop in props)
        {
            // Ignorar propriedades que estão na lista de exclusão
            if (excludeProps.Contains(prop.Name))
                continue;

            setClauses.Add($"{prop.Name.ToLower()} = @{prop.Name}");
        }

        string updateSQL = @$"UPDATE {tableName} SET {string.Join(", ", setClauses)} 
        WHERE {conditionColumn.ToLower()} = @{conditionColumn}";

        return updateSQL;
    }

    public static string CreateSelectByIdQuery(string tableName, string idColumn = "Id")
    {
        return $"SELECT * FROM {tableName} WHERE {idColumn.ToLower()} = @{idColumn}";
    }

    public static string CreateDeleteQuery(string tableName, string conditionColumn)
    {
        return $"DELETE FROM {tableName} WHERE {conditionColumn.ToLower()} = @{conditionColumn};";
    }
}
