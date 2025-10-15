
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class InvalidAmountForPayment : GestaoDeBarbeariaException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public InvalidAmountForPayment(string message) : base(message)
    {

    }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
