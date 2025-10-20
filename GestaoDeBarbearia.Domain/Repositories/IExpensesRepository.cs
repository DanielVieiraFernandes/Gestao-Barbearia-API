using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination.Expenses;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IExpensesRepository
{
    Task<Expense> Create(Expense expense);
    Task<Expense?> FindById(long id);
    Task<List<Expense>> GetAll(RequestExpensesPaginationParamsJson? pagination = null);
    Task<Expense> Save(Expense expense);
    Task<decimal> GetTotalAmount(DateTime startDate, DateTime endDate);
}
