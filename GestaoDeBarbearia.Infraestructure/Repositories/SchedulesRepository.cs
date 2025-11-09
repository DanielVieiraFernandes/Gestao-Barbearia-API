using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Pagination.Appointments;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;
using System.Reflection;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class SchedulesRepository : ISchedulesRepository
{
    private DatabaseQueryBuilder databaseQueryBuilder;
    public SchedulesRepository(DatabaseQueryBuilder dbFunctions)
    {
        this.databaseQueryBuilder = dbFunctions;
    }

    public async Task Create(Appointment appointment, List<Service> services)
    {
        await using NpgsqlConnection connection = await databaseQueryBuilder.CreateNewConnection();

        StringBuilder sql = new();

        // CALCULA O PREÇO TOTAL COM BASE NOS SERVIÇOS
        appointment.ServicePrice = services.Sum(s => s.Price);

        sql.Clear();

        List<string> propriedadesIgnorar = [
            nameof(Appointment.Id).ToLower(),
            nameof(Appointment.Services).ToLower(),
            nameof(Appointment.CreatedAt).ToLower(),
            nameof(Appointment.UpdatedAt).ToLower()
            ];

        sql.Append(DatabaseQueryBuilder.CreateInsertQuery<Appointment>
            (
            "barber_shop_appointments",
            propriedadesIgnorar)
            );
        sql.Append(" RETURNING *");

        var result = await connection.QuerySingleOrDefaultAsync<Appointment>(sql.ToString(), appointment);

        if (result is null)
            throw new ErrorRegisteringAppointmentException("Erro ao registrar agendamento");

        sql.Clear();

        sql.Append("INSERT INTO barber_shop_appointments_services (appointmentid, serviceid) VALUES ");

        services.ForEach(s =>
        {
            sql.Append($"({result.Id},{s.Id})");

            if (s != services.Last())
                sql.Append(',');
        });

        await connection.ExecuteAsync(sql.ToString());
    }

    public async Task<List<Appointment>> FilterByMonth(DateTime startDate, DateTime endDate)
    {
        await using var connection = await databaseQueryBuilder.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("SELECT * FROM barber_shop_appointments WHERE (appointmentdatetime BETWEEN @StartDate ");
        sql.Append("AND @EndDate) ");
        sql.Append($"AND status = {(int)AppointmentStatus.Completed}");

        var appointments = await connection.QueryAsync<Appointment>(sql.ToString(), new
        {
            StartDate = startDate,
            EndDate = endDate
        });

        if (appointments is null)
            throw new NotFoundException("Serviços não encontrados");

        return [.. appointments];
    }

    public async Task<List<Appointment>> FindAll(RequestAppointmentsPaginationParamsJson pagination)
    {
        await using var connection = await databaseQueryBuilder.CreateNewConnection();

        StringBuilder sql = new();

        DynamicParameters parameters = new();

        PropertyInfo[] props = typeof(Appointment).GetProperties();

        List<string> appointmentColumns = props.Where(p => !p.Name.Equals("services", StringComparison.CurrentCultureIgnoreCase)).Select(p => "ap." + p.Name.ToLower()).ToList();

        props = typeof(Service).GetProperties();

        List<string> serviceColumns = props.Select(p => "s." + p.Name.ToLower()).ToList();

        sql.Append($"SELECT {string.Join(',', appointmentColumns)}, {string.Join(',', serviceColumns)} ");
        sql.Append($"FROM barber_shop_appointments AS ap INNER JOIN barber_shop_appointments_services AS aps ");
        sql.Append("ON ap.id = aps.appointmentid ");
        sql.Append("INNER JOIN barber_shop_services AS s ON aps.serviceid = s.id ");
        sql.Append("WHERE ap.id <> 0 ");

        if (pagination.Status is not null)
        {
            sql.Append("AND ap.status = @Status ");
            parameters.Add("Status", pagination.Status);
        }

        sql.Append($"ORDER BY ap.{pagination.OrderByColumn.ToString().ToLower()} {pagination.OrderByDirection.GetEnumDescription()} ");

        Dictionary<long, Appointment> appointmentsDictionary = [];

        var result = await connection.QueryAsync<Appointment, Service, Appointment>(sql.ToString(), (appointment, service) =>
        {
            // Caso haja mais de um serviço para o mesmo agendamento
            // é necessário agrupar os serviços dentro do agendamento
            // para evitar duplicidade de agendamentos na lista final
            // Ex: Agendamento 1 -> Serviço A, Serviço B

            if (!appointmentsDictionary.TryGetValue(appointment.Id, out var currentAppointment))
            {
                currentAppointment = appointment;
                currentAppointment.Services = [];
                appointmentsDictionary.Add(currentAppointment.Id, currentAppointment);
            }

            currentAppointment.Services.Add(service);

            return currentAppointment;
        }, splitOn: "id");

        if (result is null)
            return [];

        return [.. appointmentsDictionary.Values];
    }


    public async Task<Appointment?> FindById(long id)
    {
        await using NpgsqlConnection connection = await databaseQueryBuilder.CreateNewConnection();

        string sql = "SELECT * FROM barber_shop_appointments WHERE id = @Id";

        return await connection.QuerySingleOrDefaultAsync<Appointment>(sql.ToString(), new { id });
    }

    public async Task<bool> IsTimeSlotOccupied(DateTime appointmentDateTime, int durationOfService, long employeeId)
    {
        var appointmentStart = appointmentDateTime;
        var appointmentEnd = appointmentDateTime.AddMinutes(durationOfService);

        /*
         *  -> A DATA DO AGENDAMENTO EM SI DEVE SER MAIOR QUE QUALQUER DATA DE TÉRMINO 
         *  DO SERVIÇO.
         *  
         *  -> A DATA DE TÉRMINO ESTIMADA DESSE AGENDAMENTO DEVE SER MENOR
         *  QUE A DATA DE AGENDAMENTO DE QUALQUER OUTRO SERVIÇO
         * 
         */

        string sql = @"
        SELECT
            COUNT(*)
        FROM
            barber_shop_appointments
        WHERE
            employeeId = @EmployeeId
            AND appointmentenddatetime > @AppointmentStart AND appointmentdatetime < @AppointmentEnd ";

        var parameters = new
        {
            EmployeeId = employeeId,
            AppointmentStart = appointmentStart,
            AppointmentEnd = appointmentEnd
        };

        await using NpgsqlConnection connection = await databaseQueryBuilder.CreateNewConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, parameters);

        return count > 0;
    }
    public async Task Update(Appointment appointment)
    {
        await using NpgsqlConnection connection = await databaseQueryBuilder.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("UPDATE barber_shop_appointments SET appointmentdatetime = @AppointmentDateTime, clientname = @ClientName, ");
        sql.Append("clientphone = @ClientPhone, paymenttype = @PaymentType, status = @Status, paidat = @PaidAt ,updatedat = @UpdatedAt, observations = @Observations WHERE id = @Id ");

        await connection.ExecuteAsync(sql.ToString(), appointment);
    }
}
