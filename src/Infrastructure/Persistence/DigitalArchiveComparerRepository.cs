using System.Text.Json;
using BANHCAFE.Cross.DBConnection;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Extensions;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Ports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Persistence;

public class DigitalArchiveComparerRepository(
    ILogger<DigitalArchiveComparerRepository> logger,
    ISqlDbConnectionApiExtensions<object, IEnumerable<DigitalArchiveComparerBase>> api,
    IOptionsMonitor<DatabaseSettings> dbSettingsMonitor
) : IDigitalArchiveComparerRepository
{
    private readonly DatabaseSettings _dbSettings = dbSettingsMonitor.Get(
        DatabaseSettingsInstances.SQL
    );

    internal const string QueryCommand = "SP_SELALL_COREADCOMPARER";
    internal const string PopulateCommand = "SP_INS_ONBASECOREDATA";
    internal const string GetLastMigrationData = "SP_SEL_LASTPROCESS";

    public async Task<IEnumerable<DigitalArchiveComparerBase>> List(
        ViewDigitalArchiveComparerDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var request = new SqlDbApiRequest<object>
        {
            Scheme = _dbSettings.SchemeName,
            Database = _dbSettings.DatabaseName,
            StoredProcedure = QueryCommand,
            Parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(
                    dto,
                    options: new()
                    {
                        DefaultIgnoreCondition = System
                            .Text
                            .Json
                            .Serialization
                            .JsonIgnoreCondition
                            .WhenWritingNull
                    }
                )
            )!,
        };

        var response = await api.Process(logger, request, cancellationToken);
        return response;
    }

    public async Task<DigitalArchiveComparerBase> View(
        ViewDigitalArchiveComparerDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var request = new SqlDbApiRequest<object>
        {
            Scheme = _dbSettings.SchemeName,
            Database = _dbSettings.DatabaseName,
            StoredProcedure = QueryCommand,
            Parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(
                    dto,
                    options: new()
                    {
                        DefaultIgnoreCondition = System
                            .Text
                            .Json
                            .Serialization
                            .JsonIgnoreCondition
                            .WhenWritingNull
                    }
                )
            )!,
        };
        var response = await api.Process(logger, request, cancellationToken);
        return response.FirstOrDefault();
    }

    public async Task<DigitalArchiveComparerBase> Populate(
        PopulateDataDigitalArchiveComparerDto dto,
        CancellationToken cancellationToken
    )
    {
        var request = new SqlDbApiRequest<object>
        {
            Scheme = _dbSettings.SchemeName,
            Database = _dbSettings.DatabaseName,
            StoredProcedure = PopulateCommand,
            Parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(
                    dto,
                    options: new()
                    {
                        DefaultIgnoreCondition = System
                            .Text
                            .Json
                            .Serialization
                            .JsonIgnoreCondition
                            .WhenWritingNull
                    }
                )
            )!,
        };

        var response = await api.Process(logger, request, cancellationToken);
        return response.FirstOrDefault();
    }
}
