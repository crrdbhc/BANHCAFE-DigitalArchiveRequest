using System.Net;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Endpoints.Filters;

public class FeatureGateEndpointFilter(IFeatureManager featureManager) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        if (context.HttpContext.GetEndpoint() is { } endpoint)
        {
            if (endpoint.Metadata.GetMetadata<FeatureGateAttribute>() is { } metadata)
            {
                var features = metadata.Features;

                if (metadata.RequirementType == RequirementType.Any)
                {
                    foreach (var feature in features)
                    {
                        if (await featureManager.IsEnabledAsync(feature))
                        {
                            return await next(context);
                        }
                    }
                    return Results.StatusCode((int)HttpStatusCode.NotFound);
                }
                else
                {
                    foreach (var feature in features)
                    {
                        if (!await featureManager.IsEnabledAsync(feature))
                        {
                            return Results.StatusCode((int)HttpStatusCode.NotFound);
                        }
                    }
                }
            }
        }
        return await next(context);
    }
}
