
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class InvalidDataToCreateExpense : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    private List<string> Errors = [];
    public InvalidDataToCreateExpense(List<string> errors) : base(null)
    {
        Errors = errors;
    }
    public override List<string> GetErrors()
    {
        return Errors;
    }
}
