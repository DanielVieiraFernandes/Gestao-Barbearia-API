
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class AppointmentAlreadyConfirmedException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public AppointmentAlreadyConfirmedException(string message) : base(message)
    {

    }
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
