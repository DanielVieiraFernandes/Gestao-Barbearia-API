using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;
using GestaoDeBarbearia.Infraestructure.Utils;
using Npgsql;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ScheduleRepository : IScheduleRepository
{
    private DBFunctions dbFunctions;
    public ScheduleRepository(DBFunctions dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task Create(Appointment appointment, List<long> ServiceIds)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

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

    public Task<List<Appointment>> FindAll()
    {
        throw new NotImplementedException();
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
        sql.Append("clientphone = @ClientPhone, paymenttype = @PaymentType, status = @Status WHERE id = @Id ");

        await connection.ExecuteAsync(sql.ToString(), appointment);
    }
}
