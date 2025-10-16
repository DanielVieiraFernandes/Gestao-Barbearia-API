using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Expenses;
public class GetExpenseByIdUseCase
{
    private readonly IExpensesRepository expensesRepository;
    public GetExpenseByIdUseCase(IExpensesRepository expensesRepository)
    {
        this.expensesRepository = expensesRepository;
    }
    public async Task<Expense> Execute(long id)
    {
        var expense = await expensesRepository.FindById(id);

        if (expense is null)
            throw new NotFoundException("Despesa não encontrada");

        return expense;
    }
}
