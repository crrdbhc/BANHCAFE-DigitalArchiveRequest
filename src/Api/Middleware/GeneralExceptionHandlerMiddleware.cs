using System.Text.Json;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Middleware;

public class GlobalExceptionHandlerMiddleware(
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    RequestDelegate next
)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException ex) when (ex.InnerException is JsonException exception)
        {
            var traceId = Guid.NewGuid();
            logger.LogError(
                "Error occure while processing the request, TraceId : {TraceId} Ex: {@ex}",
                traceId,
                ex
            );

            var apiResponse = new ApiResponse<object>();
            apiResponse.AddErrors(
                $"Error al leer parametro {exception.Path}. Verificar tipo de dato."
            );
            apiResponse.AddMetadata("traceId", traceId);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(apiResponse);
            await context.Response.StartAsync();
            return;
        }
        catch (Exception ex)
        {
            var traceId = Guid.NewGuid();
            logger.LogError(
                "Error occure while processing the request, TraceId : {TraceId} Ex: {@ex}",
                traceId,
                ex
            );

            var apiResponse = new ApiResponse<object>();
            apiResponse.AddErrors("Error de servidor");
            apiResponse.AddMetadata("traceId", traceId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(apiResponse);
            await context.Response.StartAsync();
            return;
        }
    }
}
