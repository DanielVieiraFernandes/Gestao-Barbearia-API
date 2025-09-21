using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
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

    public async Task Create(Appointment appointment)
    {
        await using NpgsqlConnection connection = await dbFunctions.CreateNewConnection();
        try
        {

            StringBuilder sql = new();

            sql.Append("INSERT INTO barber_shop_appointments (appointmentdatetime, clientId, clientname, ");
            sql.Append("clientphone,employeeId, serviceprice, paymenttype) ");
            sql.Append("VALUES (@AppointmentDateTime, @ClientId, @ClientName, @ClientPhone, @EmployeeId, ");
            sql.Append("@ServicePrice, @PaymentType);");

            await connection.ExecuteAsync(sql.ToString(), appointment);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public Task<List<Appointment>> FindAll()
    {
        throw new NotImplementedException();
    }

    public Task<Appointment?> FindById(long id)
    {
        throw new NotImplementedException();
    }

    public Task Update()
    {
        throw new NotImplementedException();
    }
}
