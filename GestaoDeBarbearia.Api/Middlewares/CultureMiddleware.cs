
using System.Globalization;

namespace GestaoDeBarbearia.Api.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate next;

    public CultureMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // DEFINE A LÍNGUA PORTUGUESA FALADA NO BRASIL COMO PADRÃO
        CultureInfo.CurrentCulture = new("pt-BR");
        CultureInfo.CurrentUICulture = new("pt-BR");

        await next(context);
    }

}
