using System.Reflection;
using System.Security.Cryptography;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Behaviours;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core;

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

        _ = services.AddAutoMapper(c =>
            c.AddProfile(new ComparerCoreAD.Mapper.AutoMapperProfile())
        );

        _ = services.AddAutoMapper(c => c.AddProfile(new Migrations.Mapper.AutoMapperProfile()));

        services.AddScoped<
            IValidator<PopulateDataDigitalArchive>,
            PopulateDataDigitalArchiveComparerValidator
        >();

        return services;
    }
}
