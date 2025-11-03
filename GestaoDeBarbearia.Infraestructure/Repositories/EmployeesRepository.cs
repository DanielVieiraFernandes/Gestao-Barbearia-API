using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class EmployeesRepository : IEmployeesRepository
{
    private readonly DatabaseQueryBuilder databaseQueryBuilder;
    private const string TABLE_NAME = "barber_shop_employees";
    public EmployeesRepository(DatabaseQueryBuilder databaseQueryBuilder)
    {
        this.databaseQueryBuilder = databaseQueryBuilder;
    }

    public async Task Create(Employee employee)
    {
        await using var connection = await databaseQueryBuilder.CreateNewConnection();

        string sql = DatabaseQueryBuilder.CreateInsertQuery<Employee>(TABLE_NAME);

        await connection.ExecuteAsync(sql, employee);
    }
}
