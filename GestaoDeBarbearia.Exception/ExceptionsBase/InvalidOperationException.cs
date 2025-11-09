
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class InvalidOperationException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public InvalidOperationException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
