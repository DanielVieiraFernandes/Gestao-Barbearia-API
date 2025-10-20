using GestaoDeBarbearia.DbUp.Services;
using GestaoDeBarbearia.DbUp.Utils;
using Microsoft.Extensions.Configuration;
using Npgsql;

// =================================================================
// INICIALIZAÇÃO DA APLICAÇÃO COM LEITURA DO ARQUIVO .INI
// =================================================================

// Lê o arquivo .ini
IConfiguration config = new ConfigurationBuilder()
            .AddIniFile("dbup.ini")
            .Build();

// Busca a seção referente ao banco de dados
IConfigurationSection section = config.GetSection("Banco de Dados PostgreSQL");

// =================================================================

Console.Title = "Gerenciador do Banco de Dados";

byte op = 0;

await using NpgsqlConnection connection = new(section["ConnectionString"]);
connection.Open();

BarberShopDatabaseService barberShopDatabaseService = new(connection);

while (op != 3)
{
    Utils.ShowHeader();

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1 - CRIAR TABELAS NO BANCO (GERAL)");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("2 - CRIAR TABELAS NO BANCO (INDIVIDUAL)");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("3 - SELECIONAR TABELA PARA RODAR SEED");
    Console.ResetColor();
    Console.Write("\nEscolha uma opção: ");

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
                Console.WriteLine("----------------------------------");
                Console.WriteLine("|       Seeds Disponíveis   :)   |");
                Console.WriteLine("|                                |");
                Console.WriteLine("| 1 - RODAR SEED DE AGENDAMENTOS |");
                Console.WriteLine("| 2 - RODAR SEED DE FUNCIONÁRIOS |");
                Console.WriteLine("| 3 - RODAR SEED DE PRODUTOS     |");
                Console.WriteLine("| 4 - RODAR SEED DE DESPESAS     |");
                Console.WriteLine("----------------------------------");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("X 5 - RETORNAR AO MENU PRINCIPAL X");
                Console.ResetColor();

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
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
                            Utils.PauseAndClean();
                            continue;
                        }
                    case "2":
                        {
                            Console.Clear();
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
                            Utils.PauseAndClean();
                            continue;
                        }
                    case "3":
                        {
                            Console.Clear();
                            Console.WriteLine("Quantos registros deseja inserir?");
                            bool isAInt = int.TryParse(Console.ReadLine(), out int numberOfRecords);
                            if (!isAInt || numberOfRecords < 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Valor inválido, digite novamente: \n");
                                Console.ResetColor();
                                continue;
                            }
                            await SeedService.RunSeedInProducts(connection, numberOfRecords);
                            Utils.PauseAndClean();
                            continue;
                        }
                    case "4":
                        {
                            Console.Clear();
                            Console.WriteLine("Quantos registros deseja inserir?");
                            bool isAInt = int.TryParse(Console.ReadLine(), out int numberOfRecords);
                            if (!isAInt || numberOfRecords < 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Valor inválido, digite novamente: \n");
                                Console.ResetColor();
                                continue;
                            }
                            await SeedService.RunSeedInExpenses(connection, numberOfRecords);
                            Utils.PauseAndClean();
                            continue;
                        }
                    default:
                        Console.Clear();
                        break;
                }

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
    await barberShopDatabaseService.CreateBarberShopServicesTable();
    await barberShopDatabaseService.CreateBarberShopClientsTable();
    await barberShopDatabaseService.CreateBarberShopEmployeesTable();
    await barberShopDatabaseService.CreateBarberShopAppointmentsTable();
    await barberShopDatabaseService.CreateBarberShopAppointmentsServicesTable();
    await barberShopDatabaseService.CreateBarberShopProductsTable();
    await barberShopDatabaseService.CreateBarberShopSalesTable();
    await barberShopDatabaseService.CreateBarberShopSaleDetailsTable();
    await barberShopDatabaseService.CreateBarberShopImpostosTable();
    await barberShopDatabaseService.CreateBarberShopExpensesTable();

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("\nTabelas no banco criadas com sucesso!");
    Utils.PauseAndClean();
}

async Task CreateTablesIndividual()
{
    byte op = 0;

    while (op != 5)
    {
        Utils.ShowHeader();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("-> 11 - RETORNAR AO MENU PRINCIPAL\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Console.WriteLine("| 1  <-> CRIAR TABELA DE SERVIÇOS                           |");
        Console.WriteLine("| 2  <-> CRIAR TABELA DE AGENDAMENTOS                       |");
        Console.WriteLine("| 3  <-> CRIAR TABELA DE CLIENTES                           |");
        Console.WriteLine("| 4  <-> CRIAR TABELA DE FUNCIONÁRIOS                       |");
        Console.WriteLine("| 5  <-> CRIAR TABELA DE PRODUTOS                           |");
        Console.WriteLine("| 6  <-> CRIAR TABELA DE VENDAS                             |");
        Console.WriteLine("| 7  <-> CRIAR TABELA DE DESPESAS                           |");
        Console.WriteLine("| 8  <-> CRIAR TABELA INTERMEDIÁRIA (services_appointments) |");
        Console.WriteLine("| 9  <-> CRIAR TABELA INTERMEDIÁRIA (sale_details)          |");
        Console.WriteLine("| 10 <-> CRIAR TABELA DE IMPOSTOS (SIMPLES NACIONAL)        |");
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n\n");
        Console.ResetColor();

        Console.Write("Escolha uma opção: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopServicesTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "2":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopAppointmentsTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "3":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopClientsTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "4":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopEmployeesTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "5":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopProductsTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "6":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopSalesTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "7":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopExpensesTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "8":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopAppointmentsServicesTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "9":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopSaleDetailsTable();
                Utils.PauseAndClean();
                Console.Clear();
                break;
            case "10":
                Console.Clear();
                await barberShopDatabaseService.CreateBarberShopImpostosTable();
                Utils.PauseAndClean();
                break;
            case "11":
                op = 5;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Retornando ao menu base...");
                Console.ResetColor();
                await Task.Delay(100);
                Console.Clear();
                break;
            default:
                break;
        }
    }
}

