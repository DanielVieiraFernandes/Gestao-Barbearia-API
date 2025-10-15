using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Expenses;
public class ExpensePaymentUseCase
{
    private readonly IExpensesRepository expensesRepository;
    public ExpensePaymentUseCase(IExpensesRepository expensesRepository)
    {
        this.expensesRepository = expensesRepository;
    }

    public async Task<ResponseExpenseJson> Execute(long id, RequestExpensePaymentJson request)
    {
        var expense = await expensesRepository.FindById(id);

        if (expense is null)
            throw new NotFoundException("Despesa não encontrada");

        if (expense.Status == ExpenseStatus.Paid)
            throw new ExpenseAlreadyPaid("A despesa já está paga");

        expense.PaymentDate = request.PaymentDate;

        decimal amountAfterPayment;

        if (expense.Recurrence == Recurrence.Installments)
        {
            // Como aqui eu tenho certeza de que existirá um valor para o campo de valor da parcela
            // Faço um CAST indicando que tenho certeza de que não será nulo
            amountAfterPayment = (decimal)expense.AmountOfInstallment! - request.Amount;

            if (amountAfterPayment < 0)
                throw new InvalidAmountForPayment("Valor inválido para pagamento");

            expense.PaidAmount += request.Amount;

            if (amountAfterPayment == 0)
                expense.PaidInstallments += 1;

            if (expense.PaidInstallments == expense.TotalInstallments)
                expense.Status = ExpenseStatus.Paid;
            else
                expense.Status = ExpenseStatus.InstallmentsRemaining;
        }

        if (expense.Recurrence == Recurrence.Single)
        {
            amountAfterPayment = expense.Amount - request.Amount;

            if (amountAfterPayment < 0)
                throw new InvalidAmountForPayment("Valor inválido para pagamento");

            if (amountAfterPayment > 0)
            {
                expense.Amount = amountAfterPayment;
                expense.Status = ExpenseStatus.Pending;
            }

            if (amountAfterPayment == 0)
                expense.Status = ExpenseStatus.Paid;
        }

        expense.Notes = request.Notes;

        var updatedExpense = await expensesRepository.Save(expense);

        return new ResponseExpenseJson { expense = updatedExpense };
    }
}
