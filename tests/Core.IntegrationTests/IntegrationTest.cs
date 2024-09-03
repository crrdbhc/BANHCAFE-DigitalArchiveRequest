using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.IntegrationTests;

public partial class IntegrationTest
{
    private static IServiceScopeFactory _scopeFactory = null!;
    private static WebApplicationFactory<Program> _factory = null!;

    public IntegrationTest()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }
}
