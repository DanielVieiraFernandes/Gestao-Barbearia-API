using Bogus;
using Dapper;
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
             .RuleFor(a => a.ServiceId, f => f.Random.Long(min: 1, max: 10))
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

            sql = "TRUNCATE TABLE barber_shop_appointments RESTART IDENTITY;";

            await connection.ExecuteAsync(sql.ToString());

            sql = @"
            INSERT INTO barber_shop_appointments (
              appointmentdatetime, clientId, clientname, clientphone, 
              employeeId, status, serviceprice, paymenttype, 
              paidat, createdat, observations
            ) VALUES (
              @AppointmentDateTime, @ClientId, @ClientName, @ClientPhone, 
              @EmployeeId, @Status, @ServicePrice, @PaymentType, 
              @PaidAt, @CreatedAt, @Observations
            )";

            await connection.ExecuteAsync(sql, appointments);

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

            sql = "TRUNCATE TABLE barber_shop_employees RESTART IDENTITY;";

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
}
