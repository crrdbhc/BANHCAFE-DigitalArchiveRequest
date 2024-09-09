using BANHCAFE.Cross.DBConnection;
using Refit;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Ports;

public interface ISqlDbConnectionApiExtensions<TRequest, TResponse>
    : ISqlDbConnectionApi<TRequest, TResponse>
{
    [Post("/petition/request")]
    Task<DBConnectionApiResponse<TResponse>> SendRequestAsync(
        [Body] SqlDbApiRequest<TRequest> request,
        CancellationToken cancellationToken
    );
}
