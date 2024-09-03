using System.Linq;
using System.Security.Claims;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(b =>
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();

            b.AddConfiguration(config);
        });

        builder.ConfigureServices(
            (builder, services) =>
            {
                services.Remove<IUserContext>().AddScoped<IUserContext, MockUserContext>();
            }
        );
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}

public class MockUserContext : IUserContext
{
    public MockUserContext()
    {
        User = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, "example name"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim("Id", "1"),
                    new Claim("custom-claim", "example claim value"),
                },
                "mock"
            )
        );
    }

    public ClaimsPrincipal User { get; }
}
