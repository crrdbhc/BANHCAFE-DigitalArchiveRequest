using MediatR;
using Microsoft.Extensions.Logging;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Request: {Name} {@Request}", requestName, request);

        var response = await next();

        logger.LogInformation("Response: {Name} {@response}", requestName, response);

        return response;
    }
}
