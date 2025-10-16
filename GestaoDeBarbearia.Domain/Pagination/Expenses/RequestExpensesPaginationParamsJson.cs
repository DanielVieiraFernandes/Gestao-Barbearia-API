using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Pagination.Expenses;
public class RequestExpensesPaginationParamsJson : RequestPaginationParamsJson
{
    public OrderByExpenseColumn OrderByColumn { get; set; } = OrderByExpenseColumn.DueDate;
    public ExpenseStatus? Status { get; set; }
}
