namespace GestaoDeBarbearia.DbUp.Utils;
public class Utils
{
    public static void ShowHeader()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("---- GERENCIADOR DO BANCO DE DADOS ----");
        Console.WriteLine("---------------------------------------\n");
        Console.ResetColor();
    }

    public static void PauseAndClean()
    {
        Console.ReadKey();
        Console.Clear();
    }
}
