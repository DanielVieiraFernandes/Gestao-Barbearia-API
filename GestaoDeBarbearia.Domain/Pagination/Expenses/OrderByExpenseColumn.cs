using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Pagination.Expenses;
public enum OrderByExpenseColumn
{
    [Description("Ordernar pela data de vencimento da despesa")]
    DueDate = 1,
    [Description("Ordernar pela data de pagamento da despesa")]
    PaymentDate = 2,
    [Description("Ordernar pelo status da despesa")]
    Status = 3,
    [Description("Ordernar pelo valor da despesa")]
    Amount = 4,
}
