using System.Security.Cryptography;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Endpoints.Filters;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Options;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Api.Endpoints;

public static class MigrationsEndpoints
{
    public static IEndpointRouteBuilder AddMigrationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(new(1) { })
            .ReportApiVersions()
            .Build();

        var backOfficeEndpoints = endpoints
            .MapGroup("backOffice/ad-core-comparer")
            .AddEndpointFilter<FeatureGateEndpointFilter>()
            .WithTags("MigrationsComparerCoreAD")
            .WithApiVersionSet(versionSet)
            .RequireAuthorization(AuthenticationPolicy.BackOffice);

        _ = backOfficeEndpoints
            .MapGet(
                "/last-migration",
                static async (
                    IFeatureManager features,
                    IMediator mediator,
                    [FromQuery] int? terminalId,
                    [FromQuery] int? page,
                    [FromQuery] int? size
                ) =>
                {
                    var result = await mediator.Send(new ListMigrationsQuery { });

                    if (result.ValidationErrors.Count > 0)
                    {
                        return Results.BadRequest(result);
                    }

                    if (result.Errors.Count > 0)
                    {
                        return Results.BadRequest(result);
                    }

                    return Results.Ok(result);
                }
            )
            .WithDisplayName("GetLastMigration")
            .WithName("GetLastMigration")
            .WithMetadata(new FeatureGateAttribute("BOF-get_LastMigrationData"))
            .Produces<ApiResponse<ListMigrationsQuery>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        return endpoints;
    }
}
