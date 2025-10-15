using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IExpensesRepository
{
    Task<Expense> Create(Expense expense);
    Task<Expense?> FindById(long id);
    Task<List<Expense>> GetAll();
    Task<Expense> Save(Expense expense);
}
