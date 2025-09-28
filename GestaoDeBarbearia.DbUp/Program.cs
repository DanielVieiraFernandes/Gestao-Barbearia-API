using GestaoDeBarbearia.DbUp.Services;
using GestaoDeBarbearia.DbUp.Utils;
using Npgsql;

Console.Title = "Gerenciador do Banco de Dados";

while (true)
{
    Utils.ShowHeader();

    Console.Write("Digite a senha para entrar: ");

    if (Console.ReadLine() != "c6628901d4")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Senha incorreta! tente novamente...");
        Console.ResetColor();
        Utils.PauseAndClean();
        continue;
    }

    Console.Clear();
    break;
}

byte op = 0;

await using NpgsqlConnection connection = new("Host=localhost;Database=barbearia;Port=5432;Username=postgres;Password=compras");
connection.Open();

BarberShopService barberShopService = new(connection);

while (op != 3)
{
    Utils.ShowHeader();

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1 - CRIAR TABELAS NO BANCO (GERAL)");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("2 - CRIAR TABELAS NO BANCO (INDIVIDUAL)");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("3 - RODAR SEED DE AGENDAMENTOS");
    Console.WriteLine("4 - RODAR SEED DE FUNCIONÁRTIOS\n");
    Console.ResetColor();
    Console.Write("Escolha uma opção: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Clear();
            await CreateTablesGeneral();
            break;
        case "2":
            Console.Clear();
            await CreateTablesIndividual();
            break;
        case "3":
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Quantos registros deseja inserir?");

                bool isAInt = int.TryParse(Console.ReadLine(), out int numberOfRecords);

                if (!isAInt || numberOfRecords < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Valor inválido, digite novamente: \n");
                    Console.ResetColor();
                    continue;
                }

                await SeedService.RunSeedInAppointments(connection, numberOfRecords);
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                break;
            }

            break;
        case "4":
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Quantos registros deseja inserir?");

                bool isAInt = int.TryParse(Console.ReadLine(), out int numberOfRecords);

                if (!isAInt || numberOfRecords < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Valor inválido, digite novamente: \n");
                    Console.ResetColor();
                    continue;
                }

                await SeedService.RunSeedInEmployees(connection, numberOfRecords);
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                break;
            }
            break;
        case "5":
            op = 3;
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Opção inválida!");
            Console.ResetColor();
            Utils.PauseAndClean();

            break;
    }
}

async Task CreateTablesGeneral()
{
    await barberShopService.CreateBarberShopServicesTable();
    await barberShopService.CreateBarberShopAppointmentsTable();
    await barberShopService.CreateBarberShopAppointmentsServicesTable();
    await barberShopService.CreateBarberShopClientsTable();
    await barberShopService.CreateBarberShopEmployeesTable();

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("\nTabelas no banco criadas com sucesso!");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
    Utils.PauseAndClean();
}

async Task CreateTablesIndividual()
{
    byte op = 0;

    while (op != 5)
    {
        Utils.ShowHeader();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("-> 8 - RETORNAR AO MENU PRINCIPAL\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
        Console.WriteLine("| 1 - CRIAR TABELA DE SERVIÇOS        |");
        Console.WriteLine("| 2 - CRIAR TABELA DE AGENDAMENTOS    |");
        Console.WriteLine("| 3 - CRIAR TABELA DE CLIENTES        |");
        Console.WriteLine("| 4 - CRIAR TABELA DE FUNCIONÁRIOS    |");
        Console.WriteLine("| 5 - CRIAR TABELA DE PRODUTOS        |");
        Console.WriteLine("| 6 - CRIAR TABELA DE DESPESAS        |");
        Console.WriteLine("| 7 - CRIAR TABELA INTERMEDIÁRIA (sa) |");
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++\n\n");
        Console.ResetColor();

        Console.Write("Escolha uma opção: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Clear();
                await barberShopService.CreateBarberShopServicesTable();
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "2":
                Console.Clear();
                await barberShopService.CreateBarberShopAppointmentsTable();
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "3":
                Console.Clear();
                await barberShopService.CreateBarberShopClientsTable();
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "4":
                Console.Clear();
                await barberShopService.CreateBarberShopEmployeesTable();
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "7":
                Console.Clear();
                await barberShopService.CreateBarberShopAppointmentsServicesTable();
                Console.WriteLine("Pressione qualquer tecla para retornar ao menu de criação de tabelas individuais.");
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "8":
                op = 5;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Retornando ao menu base...");
                Console.ResetColor();
                Utils.PauseAndClean();
                break;
            default:
                break;
        }
    }
}

