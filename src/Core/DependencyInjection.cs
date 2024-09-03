using System.Reflection;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Behaviours;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core;

public static class DependencyInjection
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        _ = services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            _ = cfg.AddBehavior(
                typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehaviour<,>)
            );
        });
        services.AddScoped<IValidator<CreateTodoRequest>, CreateTodoRequestValidator>();

        return services;
    }
}
