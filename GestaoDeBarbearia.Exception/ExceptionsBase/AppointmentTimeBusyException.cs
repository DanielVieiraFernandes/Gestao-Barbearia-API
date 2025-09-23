
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class AppointmentTimeBusyException : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public AppointmentTimeBusyException(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
