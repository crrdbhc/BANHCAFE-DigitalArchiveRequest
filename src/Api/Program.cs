using Asp.Versioning;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Endpoints;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Middleware;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Options;
using Banhcafe.Microservices.ComparerCoreVsAD.Core;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure;
using Microsoft.FeatureManagement;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder
        .Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        })
        .AddApiExplorer(options =>
        {
            // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

    builder.Configuration.AddEnvironmentVariables();

    #region logging
    builder.AddLoggingOptions();
    #endregion logging

    builder.Host.UseNacosConfig(section: "NacosConfig");

    builder.Services.Configure<DatabaseSettings>(
        DatabaseSettingsInstances.SQL,
        builder.Configuration.GetSection(DatabaseSettingsInstances.SQL)
    );

    //builder.Services.Configure<DatabaseSettings>(
    //    builder.Configuration.GetSection(nameof(DatabaseSettings))
    //);

    builder.Services.Configure<JWTSettings>(
        JWTSettingsSections.External,
        builder.Configuration.GetSection(JWTSettingsSections.External)
    );
    builder.Services.Configure<JWTSettings>(
        JWTSettingsSections.Internal,
        builder.Configuration.GetSection(JWTSettingsSections.Internal)
    );

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHealthChecks();
    builder.Services.AddFeatureManagement();

    #region distributed-tracing
    builder.Services.AddTracingOptions(builder.Configuration);
    builder.Services.AddMetricsOptions(builder.Configuration);
    #endregion distributed-tracing

    #region services
    builder.Services.RegisterCoreServices();
    builder.Services.RegisterInfrastructureServices(builder.Configuration);
    #endregion services

    #region options
    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddAuthenticationOptions(builder.Configuration);
    builder.Services.AddSwaggerOptions();
    #endregion options

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseAuthorization();
    app.UseAuthentication();

    #region endpoints
    app.MapHealthChecks("/health").AllowAnonymous();
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    app.AddDigitalArchiveComparerEndpoints();
    app.AddMigrationsEndpoints();
    #endregion endpoints

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            // Build a swagger endpoint for each discovered API version
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown");
    Log.CloseAndFlush();
}

public partial class Program { }
