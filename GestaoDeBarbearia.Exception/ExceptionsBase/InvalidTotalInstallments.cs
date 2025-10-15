
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class InvalidTotalInstallments : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public InvalidTotalInstallments(string message) : base(message)
    {

    }
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
