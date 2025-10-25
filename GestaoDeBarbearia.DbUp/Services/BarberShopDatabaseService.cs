using Dapper;
using GestaoDeBarbearia.DbUp.Models;
using GestaoDeBarbearia.DbUp.Utils;
using GestaoDeBarbearia.Domain.Entities;
using Npgsql;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace GestaoDeBarbearia.DbUp.Services;
internal class BarberShopDatabaseService
{
    private readonly NpgsqlConnection connection;
    private readonly string FilePath;

    private void LogErro(Exception ex, [CallerMemberName] string metodo = "")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Exceção no método {metodo}. Erro: \n{ex.Message}");
        Console.ResetColor();
    }

    public BarberShopDatabaseService(NpgsqlConnection connection)
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
            sql.Append("price NUMERIC(15,2) NOT NULL, ");
            sql.Append("durationminutes INT NOT NULL, ");
            sql.Append("isactive BOOLEAN DEFAULT TRUE");
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

                sql.Append("INSERT INTO barber_shop_services (name, description, price, durationminutes) VALUES ");

                DynamicParameters parametros = new();

                int paramId = 1;

                foreach (var servico in servicosJson)
                {
                    string nameParamName = $"@name{paramId}";
                    string descriptionParamName = $"@description{paramId}";
                    string priceParamName = $"@price{paramId}";
                    string durationParamName = $"@duration{paramId}";

                    sql.Append($"({nameParamName}, {descriptionParamName}, {priceParamName}, {durationParamName})");

                    parametros.Add(nameParamName, servico.name);
                    parametros.Add(descriptionParamName, servico.description);
                    parametros.Add(priceParamName, servico.price);
                    parametros.Add(durationParamName, servico.durationMinutes);

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
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopAppointmentsServicesTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela intermediária de agendamentos e serviços...");
        Console.ResetColor();

        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_appointments_services CASCADE;");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_appointments_services ( ");
            sql.Append("appointmentid BIGINT NOT NULL, ");
            sql.Append("serviceid BIGINT NOT NULL ");
            sql.Append(");");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela intermediária de agendamentos e serviços criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
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
            sql.Append("appointmentenddatetime TIMESTAMP WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("clientId BIGINT NULL DEFAULT NULL, ");
            sql.Append("clientname VARCHAR(200) NULL DEFAULT NULL, ");
            sql.Append("clientphone VARCHAR(15) NULL DEFAULT NULL, ");
            sql.Append("employeeId BIGINT NOT NULL, ");
            sql.Append("status INT NOT NULL DEFAULT 1, ");
            sql.Append("serviceprice NUMERIC(15,2) NOT NULL, ");
            sql.Append("paymenttype INT NOT NULL, ");
            sql.Append("paidat TIMESTAMP WITHOUT TIME ZONE NULL DEFAULT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(), ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(), ");
            sql.Append("observations TEXT NULL DEFAULT NULL, ");
            sql.Append("CONSTRAINT fk_appointment_clients FOREIGN KEY (clientId) REFERENCES barber_shop_clients (id) ON UPDATE CASCADE,");
            sql.Append("CONSTRAINT fk_appointment_employees FOREIGN KEY (employeeId) REFERENCES barber_shop_employees (id) ON UPDATE CASCADE ");
            sql.Append(");");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de agendamentos da barbearia criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
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
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW() ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de clientes criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
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
            sql.Append("salary NUMERIC(15,2) NOT NULL, ");
            sql.Append("position INT NOT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(), ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW() ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de funcionários criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopProductsTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de produtos...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_products CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_products ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("name VARCHAR(255) NOT NULL, ");
            sql.Append("saleprice NUMERIC(15,2) NOT NULL, ");
            sql.Append("unitcost NUMERIC(15,2) NOT NULL, ");
            sql.Append("quantity BIGINT NOT NULL, ");
            sql.Append("minimumstock INT NOT NULL, ");
            sql.Append("createdat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(), ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW() ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de produtos criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopSalesTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de vendas...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_sales CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_sales ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("saledate TIMESTAMP WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("saletotal NUMERIC(15,2) NOT NULL, ");
            sql.Append("updatedat TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW() ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de vendas criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopSaleDetailsTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de detalhes da venda...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_sale_details CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_sale_details ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("saleid BIGINT NOT NULL, ");
            sql.Append("productid BIGINT NOT NULL, ");
            sql.Append("quantity BIGINT NOT NULL, ");
            sql.Append("unitsaleprice NUMERIC(15,2) NOT NULL, ");
            sql.Append("CONSTRAINT fk_sale_details_sales FOREIGN KEY (saleid) REFERENCES barber_shop_sales (id) ON UPDATE CASCADE, ");
            sql.Append("CONSTRAINT fk_sale_details_products FOREIGN KEY (productid) REFERENCES barber_shop_products (id) ON UPDATE CASCADE ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de detalhes da venda criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopExpensesTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de despesas...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_expenses CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_expenses ( ");
            sql.Append("id BIGSERIAL PRIMARY KEY, ");
            sql.Append("duedate DATE NOT NULL, ");
            sql.Append("paymentdate TIMESTAMP WITHOUT TIME ZONE NULL DEFAULT NULL, ");
            sql.Append("amount NUMERIC(15,2) NOT NULL, ");
            sql.Append("paidamount NUMERIC(15,2) NOT NULL, ");
            sql.Append("amountofinstallment NUMERIC(15,2) NULL DEFAULT NULL, ");
            sql.Append("status INT NOT NULL DEFAULT 1, ");
            sql.Append("supplier VARCHAR(255) NOT NULL, ");
            sql.Append("notes TEXT NULL DEFAULT NULL, ");
            sql.Append("recurrence INT NOT NULL DEFAULT 1, ");
            sql.Append("totalinstallments BIGINT NULL DEFAULT NULL,");
            sql.Append("paidinstallments BIGINT NULL DEFAULT NULL ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de despesas criada com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }


    public async Task CreateBarberShopImpostosTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de impostos simples nacional...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_impostos_simples_nacional CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_impostos_simples_nacional ( ");
            sql.Append("anexo VARCHAR(9) NOT NULL,");
            sql.Append("faixa VARCHAR(1) NOT NULL,");
            sql.Append("faturamentolimiteinferior NUMERIC(9,2) NOT NULL,");
            sql.Append("faturamentolimitesuperior NUMERIC(9,2) NOT NULL,");
            sql.Append("aliquota NUMERIC(4,2) NOT NULL,");
            sql.Append("valoradeduzir NUMERIC(9,2) NOT NULL");
            sql.Append(");");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de impostos simples nacional criada com sucesso!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Inserindo dados na tabela de impostos...");
            Console.ResetColor();

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "tabelasSimplesNacional.json");

            string simplesNacionalJson = File.ReadAllText(path);

            SimplesNacional? simplesNacionalConvertido = JsonSerializer.Deserialize<SimplesNacional>(simplesNacionalJson, options);

            if (simplesNacionalConvertido is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Não foi possível carregar as informações dos impostos, verifique o arquivo e tente novamente");
                Console.ResetColor();
                return;
            }

            List<ImpostoSimplesNacional> impostos = [];

            simplesNacionalConvertido.Tabelas.ForEach(t =>
            {
                for (int i = 0; i < t.Faixas.Count; i++)
                {
                    impostos.Add(
                        new ImpostoSimplesNacional
                        {
                            Anexo = t.Anexo,
                            Aliquota = t.Faixas[i].Aliquota,
                            Faixa = t.Faixas[i].Faixa,
                            // Caso seja a faixa 1, significa que não há um limite inferior anterior a esse
                            // Logo o limite inferior é zero
                            FaturamentoLimiteInferior = t.Faixas[i].Faixa == "1" ? 0 : t.Faixas[i - 1].FaturamentoAte + 1,
                            FaturamentoLimiteSuperior = t.Faixas[i].FaturamentoAte,
                            ValorADeduzir = t.Faixas[i].ParcelaADeduzir
                        }
                    );
                }
            });

            sql.Clear();

            sql.Append(DBFunctionsDbUp.CreateInsertQuery<ImpostoSimplesNacional>("barber_shop_impostos_simples_nacional"));

            var linesImpostoCreated = await connection.ExecuteAsync(sql.ToString(), impostos);

            if (linesImpostoCreated != impostos.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Os impostos não foram gravados corretamente no banco de dados, verifique e tente novamente.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registros criados na tabela de impostos com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }

    public async Task CreateBarberShopSystemParametersTable()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Criando tabela de parâmetros do sistema e período de funcionamento...");
        Console.ResetColor();
        try
        {
            StringBuilder sql = new();

            sql.Append("DROP TABLE IF EXISTS barber_shop_system_parameters CASCADE; ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_system_parameters ( ");
            sql.Append("lunchtimestart TIME WITHOUT TIME ZONE, ");
            sql.Append("lunchtimeend TIME WITHOUT TIME ZONE, ");
            sql.Append("adminpassword VARCHAR(255) NOT NULL, ");
            sql.Append("autoconfirmappointments BOOLEAN NOT NULL DEFAULT FALSE ");
            sql.Append("); ");

            await connection.ExecuteAsync(sql.ToString());

            sql.Clear();

            sql.Append("CREATE TABLE barber_shop_operating_period ( ");
            sql.Append("dayweek INT NOT NULL, ");
            sql.Append("openinghour TIME WITHOUT TIME ZONE NOT NULL, ");
            sql.Append("closinghour TIME WITHOUT TIME ZONE NOT NULL ");
            sql.Append(");");

            await connection.ExecuteAsync(sql.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tabela de parâmetros do sistema e período de funcionamento criadas com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            LogErro(ex);
        }
    }
}
