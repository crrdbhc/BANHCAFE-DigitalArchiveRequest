using Banhcafe.Microservices.ComparerCoreVsAD.Api.Endpoints.Filters;
using Banhcafe.Microservices.ComparerCoreVsAD.Api.Options;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Commands.Create;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Api.Endpoints;

public static class DigitalArchiveComparerEndpoints
{
    public static IEndpointRouteBuilder AddDigitalArchiveComparerEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(new(1) { })
            .ReportApiVersions()
            .Build();

        var backOfficeEndpoints = endpoints
            .MapGroup("/backOffice/ad-core-comparer")
            .AddEndpointFilter<FeatureGateEndpointFilter>()
            .WithTags("ComparerCoreAD")
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
                    var result = await mediator.Send(
                        new ListDigitalArchiveComparerQuery
                        {
                            AgencyNum = agencyNum,
                            ProductNum = productNum,
                            ClientNum = clientNum,
                            Page = page,
                            Size = size
                        }
                    );

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
            .WithMetadata(new FeatureGateAttribute("BOF-get_ComparerCoreVsADData"))
            .Produces<ApiResponse<ListDigitalArchiveComparerQuery>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        _ = backOfficeEndpoints
            .MapPost(
                "/populate",
                static async (
                    IFeatureManager features,
                    IMediator mediator,
                    PopulateDataDigitalArchiveComparerCommand request
                ) =>
                {
                    var result = await mediator.Send(request);

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
            .WithDisplayName("PopulateData")
            .WithName("PopulateData")
            .WithMetadata(new FeatureGateAttribute("BOF-populate_ComparerCoreVsADData"))
            .Produces<ApiResponse<PopulateDataDigitalArchiveComparerCommand>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();
        return endpoints;
    }
}
