using Banhcafe.Microservices.DigitalArchiveRequest.Api.Endpoints.Filters;
using Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Endpoints;

public static class DigitalArchiveComparerEndpoints
{
    public static IEndpointRouteBuilder AddDigitalArchiveComparerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(new(1) { })
            .ReportApiVersions()
            .Build();

        var backOfficeEndpoints = endpoints
            .MapGroup("/backOffice/archive-digital-comparer")
            .AddEndpointFilter<FeatureGateEndpointFilter>()
            .WithTags("DigitalInfo")
            .WithApiVersionSet(versionSet)
            .RequireAuthorization(AuthenticationPolicy.BackOffice);

        _ = backOfficeEndpoints
            .MapGet(
                "",
                static async (
                    IFeatureManager features,
                    IMediator mediator,
                    [FromQuery] int? agencyNum,
                    [FromQuery] string? productNum,
                    [FromQuery] string? clientNum,
                    [FromQuery] int? terminalId,
                    [FromQuery] int? page,
                    [FromQuery] int? size
                ) =>
                {
                    var result = await mediator.Send(new ListDigitalArchiveComparerQuery { AgencyNum = agencyNum, ProductNum = productNum, ClientNum = clientNum });

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
            .WithDisplayName("GetDigitalArchiveComparerInfo")
            .WithName("GetDigitalArchiveComparerInfo")
            .WithMetadata(new FeatureGateAttribute("BOF-get_DigitalInfo"))
            .Produces<ApiResponse<ListDigitalArchiveComparerQuery>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        return endpoints;
    }
}
