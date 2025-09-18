namespace GestaoDeBarbearia.Exception;
public abstract class GestaoDeBarbeariaException : SystemException
{
    protected GestaoDeBarbeariaException(string? message) : base(message)
    {

    }

    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
