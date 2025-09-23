
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class ErrorRegisteringAppointmentException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorRegisteringAppointmentException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
