
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class ErrorRegisteringSaleException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorRegisteringSaleException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
