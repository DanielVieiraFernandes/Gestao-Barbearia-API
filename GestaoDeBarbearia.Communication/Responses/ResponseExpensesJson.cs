using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseExpensesJson
{
    public List<Expense> expenses { get; set; } = [];
}
