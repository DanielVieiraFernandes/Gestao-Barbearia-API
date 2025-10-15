using System.Reflection;

namespace GestaoDeBarbearia.Api.Utils;

/// <summary>
/// 
/// </summary>
public class RecordErrorLog
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    public static void RecordLog(System.Exception exception)
    {
        //var path = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog.txt");

        // Com isso, ele cria um arquivo de log por dia
        string fileName = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "-log.txt";

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var path = Path.Combine(directoryName!, fileName);

        string logMessage = $@"
--------------------------------------------------
[LOG DE ERRO - {DateTime.Now:yyyy-MM-dd HH:mm:ss}]
--------------------------------------------------
Origem: {exception.Source ?? "N/A"}
Tipo: {exception.GetType().FullName}
Mensagem: {exception.Message}
Detalhes do StackTrace: 
{exception.StackTrace ?? "Sem Stack Trace disponível."}
{Environment.NewLine}";

        File.AppendAllText(path, logMessage);
    }

}
