using System.Reflection;

namespace GestaoDeBarbearia.DbUp.Utils;
public class DBFunctionsDbUp
{
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

    //public static string CreateCreateTableQuery<T>(string tableName, List<string> excludeProps)
    //{
    //    PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    //    List<string> columns = [];

    //    foreach (var prop in props)
    //    {
    //        // Caso essa propriedade esteja dentro da lista, não irei incluir na query final
    //        if (excludeProps.Contains(prop.Name))
    //            continue;




    //        columns.Add(prop.Name.ToLower());
    //    }

    //    return $"CREATE TABLE {tableName} ({string.Join});";
    //}
}
