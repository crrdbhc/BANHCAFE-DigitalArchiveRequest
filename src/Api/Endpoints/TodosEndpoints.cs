using Banhcafe.Microservices.DigitalArchiveRequest.Api.Endpoints.Filters;
using Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;
using MediatR;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Endpoints;

public static class TodosEndpoints
{
    public static IEndpointRouteBuilder AddTodosEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints
            .NewApiVersionSet()
            .HasApiVersion(new(1) { })
            .ReportApiVersions()
            .Build();

        var backOfficeEndpoints = endpoints
            .MapGroup("/backOffice/todos")
            .AddEndpointFilter<FeatureGateEndpointFilter>()
            .WithTags("Todos")
            .WithApiVersionSet(versionSet)
            .RequireAuthorization(AuthenticationPolicy.BackOffice);

        _ = backOfficeEndpoints
            .MapPost(
                "",
                static async (
                    IFeatureManager features,
                    IMediator mediator,
                    CreateTodoRequest request
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
            .WithDisplayName("CreateTodo")
            .WithName("CreateTodo")
            .WithMetadata(new FeatureGateAttribute("BOF-create_todo"))
            .Produces<ApiResponse<CreateTodoRequest>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem();

        return endpoints;
    }
}
