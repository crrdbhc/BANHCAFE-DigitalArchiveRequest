using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;

public static class TracingOptions
{
    public static IServiceCollection AddTracingOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var applicationName = "DigitalArchiveRequest";
        var applicationVersion = configuration["ApplicationVersion"] ?? "1";

        services
            .AddOpenTelemetry()
            .WithTracing(config =>
            {
                config
                    .SetResourceBuilder(
                        ResourceBuilder
                            .CreateDefault()
                            .AddService(
                                serviceName: applicationName,
                                serviceVersion: applicationVersion
                            )
                    )
                    .AddSource(applicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                config.AddZipkinExporter();
                services.Configure<ZipkinExporterOptions>(configuration.GetSection("Zipkin"));
            });

        return services;
    }
}
