using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Communication.Responses;
public class ResponseExpenseJson
{
    public Expense expense { get; set; } = default!;
}
