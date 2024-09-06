using System.Reflection;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Behaviours;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Models;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Validators;
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

        _ = services.AddAutoMapper(c => c.AddProfile(new DigitalInfo.Mapper.AutoMapperProfile()));

        services.AddScoped<
            IValidator<PopulateDataDigitalArchive>,
            PopulateDataDigitalArchiveComparerValidator
        >();

        return services;
    }
}
