
using System.Net;

namespace GestaoDeBarbearia.Exception.ExceptionsBase;
public class ErrorOnValidationException : GestaoDeBarbeariaException
{
    private List<string> Errors { get; set; }
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<string> errorMessages) : base(null)
    {
        Errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return Errors;
    }
}
