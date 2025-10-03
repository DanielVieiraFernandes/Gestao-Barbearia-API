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
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
        Console.ResetColor();
    }
}
