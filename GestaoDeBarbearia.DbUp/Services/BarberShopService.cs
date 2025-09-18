using Dapper;
using GestaoDeBarbearia.DbUp.Models;
using Npgsql;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GestaoDeBarbearia.DbUp.Services;
internal class BarberShopService
{
    private readonly NpgsqlConnection connection;
    private readonly string FilePath;

    public BarberShopService(NpgsqlConnection connection)
    {
        this.connection = connection;

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        FilePath = Path.Combine(directoryName!, "Data", "servicos.json");
    }

    public async Task CreateBarberShopServicesTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de serviços da barbearia...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_services CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_services ( ");
            sql.Append("id SERIAL PRIMARY KEY, ");
            sql.Append("name VARCHAR(100) NOT NULL, ");
            sql.Append("description VARCHAR(255) NOT NULL, ");
            sql.Append("price BIGINT NOT NULL, ");
            sql.Append("active BOOLEAN DEFAULT TRUE");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de serviços criada! ");
            Console.ResetColor();

            if (File.Exists(FilePath))
            {
                sql.Clear();

                var json = File.ReadAllText(FilePath);

                List<ServicoJson>? servicosJson = JsonSerializer.Deserialize<List<ServicoJson>>(json);

                if (servicosJson is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erro ao ler arquivo JSON - Dentro do método 'CreateBarberShopServicesTable'");
                    Console.WriteLine("Tabela criada porém não foi preenchida\n");
                    Console.ResetColor();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Preenchendo tabela de serviços...");
                Console.ResetColor();

                sql.Append("INSERT INTO barber_shop_services (name, description, price) VALUES ");

                DynamicParameters parametros = new();

                int paramId = 1;

                foreach (var servico in servicosJson)
                {
                    string nameParamName = $"@name{paramId}";
                    string descriptionParamName = $"@description{paramId}";
                    string priceParamName = $"@price{paramId}";

                    sql.Append($"({nameParamName}, {descriptionParamName}, {priceParamName})");

                    parametros.Add(nameParamName, servico.name);
                    parametros.Add(descriptionParamName, servico.description);
                    parametros.Add(priceParamName, servico.price);

                    if (paramId < servicosJson.Count)
                        sql.Append(", ");
                    else
                        sql.Append("; ");

                    paramId++;
                }

                await connection.ExecuteAsync(sql.ToString(), parametros);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tabela de serviços da barbearia preenchida com sucesso!");
                Console.ResetColor();
            }

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exceção no método 'CreateBarberShopServicesTable'. Erro: \n" + ex.Message);
            Console.ResetColor();
        }
    }

    public async Task CreateBarberShopAppointmentsTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de agendamentos...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_appointments CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_appointments (");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("appointmentdatetime TIMESTAMP WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("serviceId BIGINT NOT NULL, ");
            sql.Append("clientId BIGINT NULL DEFAULT NULL, ");
            sql.Append("clientname VARCHAR(200) NULL DEFAULT NULL, ");
            sql.Append("clientphone VARCHAR(15) NULL DEFAULT NULL, ");
            sql.Append("employeeId BIGINT NULL DEFAULT NULL, ");
            sql.Append("status INT NOT NULL DEFAULT 1, ");
            sql.Append("serviceprice BIGINT NOT NULL, ");
            sql.Append("paymenttype INT NOT NULL, ");
            sql.Append("paidat TIMESTAMP WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("observations TEXT NULL DEFAULT NULL, ");
            sql.Append("CONSTRAINT fk_appointment_clients FOREIGN KEY (clientId) REFERENCES barber_shop_clients (id) ON UPDATE CASCADE,");
            sql.Append("CONSTRAINT fk_appointment_services FOREIGN KEY (serviceId) REFERENCES barber_shop_services (id) ON UPDATE CASCADE,");
            sql.Append("CONSTRAINT fk_appointment_employees FOREIGN KEY (employeeId) REFERENCES barber_shop_employees (id) ON UPDATE CASCADE ");
            sql.Append(");");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de agendamentos da barbearia criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método 'CreateBarberShopAppointmentTable'. Erro: \n" + ex.Message);
            Console.ResetColor();
        }
    }

    public async Task CreateBarberShopClientsTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de clientes...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_clients CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_clients ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("name VARCHAR(255) NOT NULL, ");
            sql.Append("telephone VARCHAR(15) UNIQUE NOT NULL, ");
            sql.Append("email VARCHAR(255) UNIQUE NOT NULL, ");
            sql.Append("password VARCHAR(255) NOT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(), ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE NOT NULL ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de clientes criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método 'CreateBarberShopClientsTable'. Erro: \n" + ex.Message);
            Console.ResetColor();
        }
    }

    public async Task CreateBarberShopEmployeesTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de funcionários...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_employees CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_employees ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("name VARCHAR(255) NOT NULL, ");
            sql.Append("telephone VARCHAR(15) UNIQUE NOT NULL, ");
            sql.Append("email VARCHAR(255) UNIQUE NOT NULL, ");
            sql.Append("password VARCHAR(255) NOT NULL, ");
            sql.Append("salary BIGINT NOT NULL, ");
            sql.Append("position INT NOT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(), ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE NOT NULL ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de funcionários criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método 'CreateBarberShopEmployeesTable'. Erro: \n" + ex.Message);
            Console.ResetColor();
        }
    }
}
