
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class NotFoundException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public NotFoundException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
