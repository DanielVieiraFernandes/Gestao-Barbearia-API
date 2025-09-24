
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class AppointmentNotConfirmedException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public AppointmentNotConfirmedException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
