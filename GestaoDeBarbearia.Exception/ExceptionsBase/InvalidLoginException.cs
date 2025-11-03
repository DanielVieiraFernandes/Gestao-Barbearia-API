
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class InvalidLoginException : GestaoDeBarbeariaException
{
    public InvalidLoginException() : base("E-mail e/ou senha inválidos")
    {

    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
