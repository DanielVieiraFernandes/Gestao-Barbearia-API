using GestaoDeBarbearia.Api.Utils;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestaoDeBarbearia.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Grava o log do erro em um arquivo de texto
        RecordErrorLog.RecordLog(context.Exception);

        // Caso seja uma instância da classe personalizada de exceção
        if (context.Exception is GestaoDeBarbeariaException)
            HandleProjectException(context);
        else
            ThrowUnknownException(context);
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var gestaoDeBarbeariaException = (GestaoDeBarbeariaException)context.Exception;
        var errorResponse = new ResponseErrorJson(gestaoDeBarbeariaException.GetErrors());

        context.HttpContext.Response.StatusCode = gestaoDeBarbeariaException.StatusCode;
        context.Result = new ObjectResult(errorResponse);

    }

    private void ThrowUnknownException(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson("UNKNOWN ERROR");

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
