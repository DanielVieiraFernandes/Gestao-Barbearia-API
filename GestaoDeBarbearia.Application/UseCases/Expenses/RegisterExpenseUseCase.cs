using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Expenses;
public class RegisterExpenseUseCase
{
    private readonly IExpensesRepository expensesRepository;

    public RegisterExpenseUseCase(IExpensesRepository expensesRepository)
    {
        this.expensesRepository = expensesRepository;
    }

    public async Task<ResponseExpenseJson> Execute(RequestExpenseJson request)
    {
        List<string> errors = [];

        decimal? amountOfInstallments = null;

        if (request.Recurrence == Recurrence.Installments)
        {

            if (request.TotalInstallments is null or <= 0)
                errors.Add("O número total de parcelas não foi informado corretamente");

            if (request.PaidInstallments is null)
                errors.Add("O número total de parcelas pagas não foi informado corretamente");

            if (errors.Count != 0)
                throw new InvalidDataToCreateExpense(errors);

            amountOfInstallments = (decimal)request.Amount / request.TotalInstallments;
        }

        if (request.Recurrence == Recurrence.Single)
        {
            if (request.TotalInstallments != null)
                errors.Add("Não deve ser informado o número de parcelas para uma despesa única");

            if (request.PaidInstallments != null)
                errors.Add("Não deve ser informado o número de parcelas pagas para uma despesa única");

            if (errors.Count != 0)
                throw new InvalidDataToCreateExpense(errors);
        }

        Expense expense = new()
        {
            DueDate = request.DueDate,
            PaymentDate = request.PaymentDate,
            Amount = request.Amount,
            TotalInstallments = request.TotalInstallments,
            AmountOfInstallment = amountOfInstallments,
            PaidAmount = request.PaidAmount,
            PaidInstallments = request.PaidInstallments,
            Notes = request.Notes,
            Recurrence = request.Recurrence,
            Supplier = request.Supplier ?? "",
        };

        expense.Status = expense.PaidAmount == expense.Amount ? Domain.Enums.ExpenseStatus.Paid : Domain.Enums.ExpenseStatus.Pending;

        // Verifica se a data atual é maior que a data de vencimento e marca como pagamento atrasado
        if (expense.DueDate < DateTime.Now && expense.Status == Domain.Enums.ExpenseStatus.Pending)
            expense.Status = Domain.Enums.ExpenseStatus.Overdue;

        var createdExpense = await expensesRepository.Create(expense);

        return new ResponseExpenseJson
        {
            expense = createdExpense,
        };
    }
}
