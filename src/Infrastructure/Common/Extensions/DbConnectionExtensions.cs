using BANHCAFE.Cross.DBConnection;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Exceptions;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Ports;
using Microsoft.Extensions.Logging;
using Refit;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Extensions;

internal static class DbConnectionExtensions
{
    public static async Task<IEnumerable<TResponse>> Process<TRequest, TResponse>(
        this ISqlDbConnectionApi<TRequest, IEnumerable<TResponse>> api,
        ILogger logger,
        SqlDbApiRequest<TRequest> request
    )
        where TResponse : new()
    {
        try
        {
            logger.LogInformation("New Request {@request}", request);

            var result = await api.SendRequestAsync(request);

            if (result.Error != "00")
            {
                logger.LogWarning("Error Response from Services {@result}", result);
                return Enumerable.Empty<TResponse>();
            }
            else
            {
                logger.LogDebug("Response from Services {@result}", result);
            }

            logger.LogDebug("DB Response {@data}", result.Data);

            return result.Data;
        }
        catch (ApiException ex)
        {
            var error = DatabaseError(logger, ex.Content ?? string.Empty);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ServiceException(error);
            }

            logger.LogWarning(
                "Services: Error while calling Database {@request} {@ex}",
                request,
                ex
            );
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Services: Unexpected error while processing database call  {@request} {@ex}",
                request,
                ex
            );

            throw;
        }

        return Enumerable.Empty<TResponse>();
    }

    public static async Task<IEnumerable<TResponse>> Process<TRequest, TResponse>(
        this ISqlDbConnectionApiExtensions<TRequest, IEnumerable<TResponse>> api,
        ILogger logger,
        SqlDbApiRequest<TRequest> request,
        CancellationToken cancellationToken
    )
        where TResponse : new()
    {
        try
        {
            logger.LogInformation("New Request {@request}", request);

            var result = await api.SendRequestAsync(request, cancellationToken);

            if (result.Error != "00")
            {
                logger.LogWarning("Error Response from Services {@result}", result);
                return Enumerable.Empty<TResponse>();
            }
            else
            {
                logger.LogInformation("Response from Services {@result}", result);
            }

            logger.LogDebug("DB Response {@data}", result.Data);

            return result.Data;
        }
        catch (ApiException ex)
        {
            var error = DatabaseError(logger, ex.Content ?? string.Empty);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ServiceException(error);
            }

            logger.LogWarning(
                "Services: Error while calling Database {@request} {@ex}",
                request,
                ex
            );
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Services: Unexpected error while processing database call  {@request} {@ex}",
                request,
                ex
            );

            throw;
        }

        return Enumerable.Empty<TResponse>();
    }

    public static async Task<IEnumerable<TResponse>> Process<TRequest, TResponse>(
        this IDb2DbConnectionApi<TRequest, IEnumerable<TResponse>> api,
        ILogger logger,
        Db2DbApiRequest<TRequest> request
    )
        where TResponse : new()
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("New Request {@request}", request);
            }

            var result = await api.SendRequestAsync(request);

            if (result.Error != "200")
            {
                logger.LogWarning("Error Response from Services {@result}", result);
                throw new ServiceException(result.Message);
            }
            else
            {
                logger.LogInformation("Response from Services {@result}", result);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("DB Response {@data}", result.Data);
            }

            return result.Data;
        }
        catch (ApiException ex)
        {
            var error = DatabaseError(logger, ex.Content ?? string.Empty);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ServiceException(error);
            }

            logger.LogWarning("Error while calling Database {@request} {@ex}", request, ex);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Unexpected error while processing database call  {@request} {@ex}",
                request,
                ex
            );

            throw;
        }

        return Enumerable.Empty<TResponse>();
    }

    private static string DatabaseError(ILogger logger, string content)
    {
        try
        {
            var payload = System.Text.Json.JsonSerializer.Deserialize<
                DBConnectionApiResponse<string>
            >(
                content,
                new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

            return payload!.Data;
        }
        catch (System.Text.Json.JsonException ex)
        {
            logger.LogError(ex, "Deserialization error");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error");
        }

        return string.Empty;
    }
}
