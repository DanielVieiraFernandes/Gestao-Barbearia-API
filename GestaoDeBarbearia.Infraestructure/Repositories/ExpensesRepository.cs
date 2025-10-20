using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Pagination.Expenses;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ExpensesRepository : IExpensesRepository
{
    private readonly DBFunctions dbFunctions;
    private const string TABLE_NAME = "barber_shop_expenses";
    public ExpensesRepository(DBFunctions dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }

    public async Task<Expense> Create(Expense expense)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateInsertQuery<Expense>(TABLE_NAME);

        var result = await connection.QueryFirstAsync<Expense>(sql + " RETURNING *", expense);

        if (result is null)
            throw new System.Exception("Erro ao criar despesa");

        return result;
    }

    public async Task<Expense?> FindById(long id)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateSelectByIdQuery(TABLE_NAME);

        var result = await connection.QueryFirstOrDefaultAsync<Expense>(sql, new { Id = id });

        return result;
    }

    public async Task<List<Expense>> GetAll(RequestExpensesPaginationParamsJson? pagination)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        sql.Append("SELECT * FROM barber_shop_expenses ");

        if (pagination != null)
        {
            if (pagination.Status != null)
                sql.Append($"WHERE status = {(int)pagination.Status} ");

            sql.Append($"ORDER BY {pagination.OrderByColumn.ToString().ToLower()} " +
                $"{pagination.OrderByDirection.GetEnumDescription()}");
        }

        var result = await connection.QueryAsync<Expense>(sql.ToString(), pagination);

        if (result is null || !result.Any())
            return [];

        return [.. result];
    }

    public async Task<Expense> Save(Expense expense)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateUpdateQuery<Expense>(TABLE_NAME);

        var result = await connection.QueryFirstAsync<Expense>(sql + " RETURNING *", expense);

        if (result is null)
            throw new SystemException("Falha ao atualizar despesa");

        return result;
    }

    public async Task<decimal> GetTotalAmount(DateTime startDate, DateTime endDate)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        DynamicParameters parameters = new();

        string sql = @$"SELECT SUM(amount) FROM barber_shop_expenses WHERE 
        (paymentdate BETWEEN @StartDate AND @EndDate) AND status = @Status";

        parameters.Add("StartDate", startDate);
        parameters.Add("EndDate", endDate);
        parameters.Add("Status", (int)ExpenseStatus.Paid);

        decimal totalAmount = await connection.ExecuteScalarAsync<decimal?>(sql, parameters) ?? 0;

        return totalAmount;
    }
}
