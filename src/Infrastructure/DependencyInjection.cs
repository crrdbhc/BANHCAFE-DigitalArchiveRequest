using System.Net;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Data;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Ports;
using Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure.Common.Ports;
using Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;
using Refit;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _ = services
            .AddHttpClient(
                "DBClient",
                static (sp, client) =>
                {
                    var dbSettings = sp.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();
                    client.BaseAddress = new Uri(dbSettings.CurrentValue.DatabaseApiUrl);
                    client.Timeout = TimeSpan.FromMinutes(3);
                }
            )
            .ConfigurePrimaryHttpMessageHandler(PrimaryHttpMessageHandler)
            .AddResilienceHandler("db-client-pipeline", BasicHttpClientResiliencePipeline);

        services.AddRefitClient<ISqlDbConnectionApiExtensions<object, IEnumerable<TodoDto>>>(
            settingsAction: (sp) => new() { },
            httpClientName: "DBClient"
        );

        services.AddScoped<ITodosRepository, TodosRepository>();

        return services;
    }

    private static HttpMessageHandler PrimaryHttpMessageHandler()
    {
        return new HttpClientHandler() { MaxConnectionsPerServer = 256, UseProxy = false };
    }

    private static void BasicHttpClientResiliencePipeline(
        ResiliencePipelineBuilder<HttpResponseMessage> builder,
        ResilienceHandlerContext context
    )
    {
        // Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
        _ = builder
            .AddRetry(
                new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 4,
                    UseJitter = true,
                    MaxDelay = TimeSpan.FromSeconds(5),
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<TimeoutRejectedException>()
                        .Handle<ApiException>()
                        .Handle<HttpRequestException>()
                        .HandleResult(static response =>
                            response.StatusCode
                                is HttpStatusCode.BadRequest
                                    or HttpStatusCode.RequestTimeout
                                    or HttpStatusCode.TooManyRequests
                                    or HttpStatusCode.InternalServerError
                                    or HttpStatusCode.BadGateway
                                    or HttpStatusCode.ServiceUnavailable
                                    or HttpStatusCode.GatewayTimeout
                        ),
                    BackoffType = DelayBackoffType.Exponential
                }
            )
            .AddTimeout(new HttpTimeoutStrategyOptions() { Timeout = TimeSpan.FromMinutes(2), });
    }
}
