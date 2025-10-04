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
        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var path = Path.Combine(directoryName!, "ErrorLog.txt");

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
