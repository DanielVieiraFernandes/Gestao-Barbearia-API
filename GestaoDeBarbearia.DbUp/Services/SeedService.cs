using Bogus;
using Dapper;
using GestaoDeBarbearia.DbUp.Utils;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using Npgsql;
using System.Reflection;

namespace GestaoDeBarbearia.DbUp.Services;
internal class SeedService
{
    public static async Task RunSeedInAppointments(NpgsqlConnection connection, int numberOfRecords)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Executando seed de agendamentos...");
            Console.ResetColor();

            var fakerAppointment = new Faker<Appointment>("pt_BR")
             .RuleFor(a => a.AppointmentDateTime, f => f.Date.Soon(days: 30))
             .RuleFor(a => a.AppointmentEndDateTime,
    (f, a) => a.AppointmentDateTime.AddMinutes(f.Random.Int(30, 90)))
             .RuleFor(a => a.ClientId, f => null)
             .RuleFor(a => a.ClientName, f => f.Person.FullName)
             .RuleFor(a => a.ClientPhone, f => f.Phone.PhoneNumber("## #####-####"))
             .RuleFor(a => a.EmployeeId, f => 1)
             .RuleFor(a => a.Status, f => f.PickRandom<AppointmentStatus>())
             .RuleFor(a => a.ServicePrice, f => f.Random.Long(min: 2500, max: 15000))
             .RuleFor(a => a.PaymentType, f => f.PickRandom<PaymentType>())
             .RuleFor(a => a.PaidAt, (f, a) => a.Status == AppointmentStatus.Completed ? f.Date.Recent() : null)
             .RuleFor(a => a.CreatedAt, f => f.Date.Recent())
             .RuleFor(a => a.Observations, f => f.Random.Words(count: 5));

            List<Appointment> appointments = fakerAppointment.Generate(numberOfRecords);

            string sql;

            sql = "TRUNCATE TABLE barber_shop_appointments RESTART IDENTITY CASCADE;";

            await connection.ExecuteAsync(sql);

            sql = @"SELECT * FROM barber_shop_services";

            var services = await connection.QueryAsync<Service>(sql);

            if (services is null || !services.Any())
                throw new Exception("Não há registros na tabela de serviços");

            foreach (var appointment in appointments)
            {
                sql = @"
                INSERT INTO barber_shop_appointments (
                  appointmentdatetime,appointmentenddatetime, clientId, clientname, clientphone, 
                  employeeId, status, serviceprice, paymenttype, 
                  paidat, createdat, observations
                ) VALUES (
                  @AppointmentDateTime, @AppointmentEndDateTime, @ClientId, @ClientName, @ClientPhone, 
                  @EmployeeId, @Status, @ServicePrice, @PaymentType, 
                  @PaidAt, @CreatedAt, @Observations
                ) RETURNING *";

                var createdAppointment = await connection.QueryFirstAsync<Appointment>(sql, appointment);

                if (createdAppointment is null)
                    throw new Exception("Erro ao criar agendamento");

                sql = @"INSERT INTO barber_shop_appointments_services (
                        appointmentid,
                        serviceid
                    ) VALUES (
                        @AppointmentId,
                        @ServiceId
                    );";

                await connection.ExecuteAsync(sql, new
                {
                    AppointmentId = createdAppointment.Id,
                    ServiceId = services.ElementAt(new Random().Next(services.Count())).Id
                });
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("seed de agendamentos executado com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método {MethodBase.GetCurrentMethod()!.Name}. Erro: \n {ex.Message}");
            Console.ResetColor();
        }
    }

    public static async Task RunSeedInEmployees(NpgsqlConnection connection, int numberOfRecords)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Executando seed de funcionários...");
            Console.ResetColor();

            var fakerEmployee = new Faker<Employee>("pt_BR")
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.Name, f => f.Person.FullName)
            .RuleFor(e => e.Telephone, f => f.Phone.PhoneNumber("## #####-####"))
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
            .RuleFor(e => e.Password, f => f.Internet.Password(length: 10, memorable: true))
            .RuleFor(e => e.Salary, f => f.Random.Long(min: 150000, max: 800000))
            .RuleFor(e => e.Position, f => f.PickRandom<EmployeePosition>())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, (f, e) => f.Date.Between(e.CreatedAt, DateTime.Now));

            List<Employee> employees = fakerEmployee.Generate(numberOfRecords);

            string sql;

            sql = "TRUNCATE TABLE barber_shop_employees RESTART IDENTITY CASCADE;";

            await connection.ExecuteAsync(sql.ToString());

            sql = @"INSERT INTO barber_shop_employees (
                            name,
                            telephone,
                            email,
                            password,
                            salary,
                            ""position"", -- A coluna 'position' deve ser entre aspas duplas, pois é uma palavra reservada no PostgreSQL
                            createdat,
                            updatedat
                        ) VALUES (
                            @Name,
                            @Telephone,
                            @Email,
                            @Password,
                            @Salary,
                            @Position,
                            @CreatedAt,
                            @UpdatedAt
                        );";

            await connection.ExecuteAsync(sql, employees);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("seed de funcionários executado com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método {MethodBase.GetCurrentMethod()!.Name}. Erro: \n {ex.Message}");
            Console.ResetColor();
        }
    }

    public static async Task RunSeedInProducts(NpgsqlConnection connection, int numberOfRecords)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Executando seed de produtos...");
            Console.ResetColor();

            var fakerProduct = new Faker<Product>("pt_BR")
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.SalePrice, f => f.Random.Long(min: 500, max: 50000))
            .RuleFor(p => p.UnitCost, (f, p) => f.Random.Decimal(min: 200, max: p.SalePrice))
            .RuleFor(p => p.Quantity, f => f.Random.Long(min: 0, max: 200))
            .RuleFor(p => p.MinimumStock, f => f.Random.Int(min: 1, max: 20))
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
            .RuleFor(p => p.UpdatedAt, (f, p) => f.Date.Between(p.CreatedAt, DateTime.Now));

            List<Product> products = fakerProduct.Generate(numberOfRecords);

            string sql;

            sql = "TRUNCATE TABLE barber_shop_products RESTART IDENTITY CASCADE;";

            await connection.ExecuteAsync(sql.ToString());

            sql = DBFunctionsDbUp.CreateInsertQuery<Product>("barber_shop_products");

            await connection.ExecuteAsync(sql, products);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("seed de produtos executado com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método {MethodBase.GetCurrentMethod()!.Name}. Erro: \n {ex.Message}");
            Console.ResetColor();
        }
    }

    public static async Task RunSeedInExpenses(NpgsqlConnection connection, int numberOfRecords)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Executando seed de despesas...");
            Console.ResetColor();

            var fakerExpense = new Faker<Expense>("pt_BR")
                .RuleFor(e => e.Id, f => f.IndexFaker + 1)
                .RuleFor(e => e.DueDate, f => f.Date.Future(0, DateTime.Now.AddMonths(2))) // próxima fatura
                .RuleFor(e => e.PaymentDate, (f, e) => f.Random.Bool(0.6f) ? e.DueDate.AddDays(f.Random.Int(-2, 3)) : null)
                .RuleFor(e => e.Amount, f => f.Finance.Amount(50, 5000))
                .RuleFor(e => e.PaidAmount, (f, e) => e.PaymentDate != null ? e.Amount : 0)
                .RuleFor(e => e.AmountOfInstallment, (f, e) => f.Random.Bool(0.3f) ? Math.Round(e.Amount / f.Random.Int(2, 12), 2) : null)
                .RuleFor(e => e.TotalInstallments, (f, e) => e.AmountOfInstallment.HasValue ? f.Random.Int(2, 12) : null)
                .RuleFor(e => e.PaidInstallments, (f, e) => e.TotalInstallments.HasValue ? f.Random.Int(0, (int)e.TotalInstallments.Value) : null)
                .RuleFor(e => e.Recurrence, f => f.PickRandom<Recurrence>())
                .RuleFor(e => e.Status, (f, e) =>
                {
                    if (e.PaymentDate.HasValue)
                        return ExpenseStatus.Paid;
                    if (e.DueDate > DateTime.Now)
                        return ExpenseStatus.Scheduled;
                    return ExpenseStatus.Pending;
                })
                .RuleFor(e => e.Supplier, f => f.Company.CompanyName())
                .RuleFor(e => e.Notes, f => f.Random.Bool(0.3f) ? f.Lorem.Sentence() : null);

            var expenses = fakerExpense.Generate(numberOfRecords);

            string sql = "TRUNCATE TABLE barber_shop_expenses RESTART IDENTITY CASCADE;";
            await connection.ExecuteAsync(sql);

            sql = DBFunctionsDbUp.CreateInsertQuery<Expense>("barber_shop_expenses");
            await connection.ExecuteAsync(sql, expenses);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Seed de despesas executado com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exceção no método {MethodBase.GetCurrentMethod()?.Name}. Erro: \n{ex.Message}");
            Console.ResetColor();
        }
    }

}
