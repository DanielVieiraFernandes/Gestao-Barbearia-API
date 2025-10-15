using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Pagination.Appointments;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class SchedulesRepository : ISchedulesRepository
{
    private DBFunctions dbFunctions;
    public SchedulesRepository(DBFunctions dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task Create(Appointment appointment, List<long> ServiceIds)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("SELECT * FROM barber_shop_services WHERE id = ANY(@ServiceIds) AND isactive = TRUE");

        var services = await connection.QueryAsync<Service>(sql.ToString(), new { ServiceIds });

        if (services is null || !services.Any())
            throw new NotFoundException("Os serviços selecionados não existem ou não estão disponíveis");

        // CALCULA O PREÇO TOTAL COM BASE NOS SERVIÇOS
        appointment.ServicePrice = services.Sum(s => s.Price);

        sql.Clear();

        sql.Append("INSERT INTO barber_shop_appointments (appointmentdatetime, clientId, clientname, ");
        sql.Append("clientphone,employeeId, serviceprice, paymenttype) ");
        sql.Append("VALUES (@AppointmentDateTime, @ClientId, @ClientName, @ClientPhone, @EmployeeId, ");
        sql.Append("@ServicePrice, @PaymentType) ");
        sql.Append("RETURNING *;");

        var result = await connection.QuerySingleOrDefaultAsync<Appointment>(sql.ToString(), appointment);

        if (result is null)
            throw new ErrorRegisteringAppointmentException("Erro ao registrar agendamento");

        sql.Clear();

        sql.Append("INSERT INTO barber_shop_appointments_services (appointmentid, serviceid) VALUES ");

        ServiceIds.ForEach(s =>
        {
            sql.Append($"({result.Id},{s})");

            if (s != ServiceIds.Last())
                sql.Append(',');
        });

        await connection.ExecuteAsync(sql.ToString());
    }

    public async Task<List<Appointment>> FilterByMonth(DateOnly month)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        DateTime startDate = new(month.Year, month.Month, 1);
        DateTime endDate = new(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));

        sql.Append("SELECT * FROM barber_shop_appointments WHERE (appointmentdatetime >= @StartDate ");
        sql.Append("AND appointmentdatetime <= @EndDate) ");
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
        await using var connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        DynamicParameters parameters = new();

        sql.Append("SELECT * FROM barber_shop_appointments WHERE id <> 0 ");

        if (pagination.Status is not null)
        {
            sql.Append("AND status = @Status ");
            parameters.Add("Status", pagination.Status);
        }

        sql.Append($"ORDER BY {pagination.OrderByColumn.ToString().ToLower()} {pagination.OrderByDirection.GetEnumDescription()} ");

        var result = await connection.QueryAsync<Appointment>(sql.ToString(), parameters);

        if (result is null)
            return [];

        return [.. result];
    }


    public async Task<Appointment?> FindById(long id)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        string sql = "SELECT * FROM barber_shop_appointments WHERE id = @Id";

        return await connection.QuerySingleOrDefaultAsync<Appointment>(sql.ToString(), new { id });
    }

    public async Task<bool> IsTimeSlotOccupied(DateTime appointmentDateTime, long employeeId)
    {
        var appointmentStart = appointmentDateTime;
        var appointmentEnd = appointmentDateTime.AddMinutes(30);

        string sql = @"
        SELECT
            COUNT(*)
        FROM
            barber_shop_appointments
        WHERE
            employeeId = @EmployeeId
            AND (
                (appointmentdatetime >= @AppointmentStart AND appointmentdatetime < @AppointmentEnd)
                OR (appointmentdatetime < @AppointmentStart AND (appointmentdatetime + INTERVAL '30 minutes') > @AppointmentStart)
            );";

        var parameters = new
        {
            EmployeeId = employeeId,
            AppointmentStart = appointmentStart,
            AppointmentEnd = appointmentEnd
        };


        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, parameters);

        return count > 0;

    }
    public async Task Update(Appointment appointment)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("UPDATE barber_shop_appointments SET appointmentdatetime = @AppointmentDateTime, clientname = @ClientName, ");
        sql.Append("clientphone = @ClientPhone, paymenttype = @PaymentType, status = @Status, paidat = @PaidAt ,updatedat = @UpdatedAt, observations = @Observations WHERE id = @Id ");

        await connection.ExecuteAsync(sql.ToString(), appointment);
    }
}
