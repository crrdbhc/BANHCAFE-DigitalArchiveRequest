using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;

public static class LogginOptions
{
    public static IHostBuilder AddLoggingOptions(this WebApplicationBuilder builder)
    {
        var applicationName = "DigitalArchiveRequest";

        return builder.Host.UseSerilog(
            (context, configuration) =>
                configuration
                    .Enrich.With<SensitiveContentEnricher>()
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithCorrelationId(
                        headerName: "x-correlation-id",
                        addValueIfHeaderAbsence: true
                    )
                    .WriteTo.Elasticsearch(
                        context.ConfigureElastiSearckSink(
                            context.Configuration["Serilog:ElasticSearchUrl"]!,
                            applicationName!
                        )
                    )
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console(new JsonFormatter())
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .MinimumLevel.Debug()
        );
    }

    private static ElasticsearchSinkOptions ConfigureElastiSearckSink(
        this HostBuilderContext context,
        string elasticSearchUrl,
        string applicationName
    ) =>
        new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
        {
            NumberOfShards = 2,
            NumberOfReplicas = 1,
            AutoRegisterTemplate = true,
            OverwriteTemplate = true,
            TemplateName = "DigitalArchiveRequest",
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            TypeName = null,
            BatchAction = ElasticOpType.Create,
            IndexFormat =
                $"{applicationName}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTimeOffset.UtcNow:yyyy-MM}",
            EmitEventFailure =
                EmitEventFailureHandling.WriteToSelfLog
                | EmitEventFailureHandling.WriteToFailureSink
                | EmitEventFailureHandling.RaiseCallback,
            DeadLetterIndexName =
                $"DL-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTimeOffset.UtcNow:yyyy-MM}",
        };

    private class SensitiveContentEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.RemovePropertyIfPresent("CVC");
            logEvent.RemovePropertyIfPresent("SSN");

            if (logEvent.Level is not LogEventLevel.Debug)
            {
                logEvent.RemovePropertyIfPresent("Documents");
            }
        }
    }
}
