using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Expenses;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Expenses;
public class FetchAllExpensesUseCase
{
    private readonly IExpensesRepository expensesRepository;
    public FetchAllExpensesUseCase(IExpensesRepository expensesRepository)
    {
        this.expensesRepository = expensesRepository;
    }

    public async Task<ResponseExpensesJson> Execute(RequestExpensesPaginationParamsJson? pagination)
    {
        var result = await expensesRepository.GetAll(pagination);

        return new ResponseExpensesJson
        {
            expenses = result,
        };
    }
}
