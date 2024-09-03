using System.Reflection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;

public static class MetricsOptions
{
    public static IServiceCollection AddMetricsOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var applicationName = "DigitalArchiveRequest";
        var applicationVersion = configuration["ApplicationVersion"] ?? "1";
        var assemblyVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

        Action<ResourceBuilder> configureResource = r =>
            r.AddService(
                applicationName,
                serviceVersion: assemblyVersion,
                serviceInstanceId: Environment.MachineName
            );

        services
            .AddOpenTelemetry()
            .WithMetrics(config =>
            {
                config
                    .ConfigureResource(configureResource)
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();
            });

        return services;
    }
}
