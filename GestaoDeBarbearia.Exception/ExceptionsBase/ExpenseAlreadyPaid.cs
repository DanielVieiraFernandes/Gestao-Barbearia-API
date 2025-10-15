
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class ExpenseAlreadyPaid : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.Conflict;

    public ExpenseAlreadyPaid(string message) : base(message)
    {

    }
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
