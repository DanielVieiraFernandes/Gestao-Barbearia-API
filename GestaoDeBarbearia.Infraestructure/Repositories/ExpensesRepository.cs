using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;

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

    public Task<List<Expense>> GetAll()
    {
        throw new NotImplementedException();
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
}
