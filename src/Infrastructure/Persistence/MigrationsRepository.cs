using System.Text.Json;
using BANHCAFE.Cross.DBConnection;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Extensions;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Ports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Persistence;

public class MigrationsRepository(
    ILogger<MigrationsRepository> logger,
    ISqlDbConnectionApiExtensions<object, IEnumerable<MigrationsBase>> api,
    IOptionsMonitor<DatabaseSettings> dbSettingsMonitor
) : IMigrationsRepository
{
    private readonly DatabaseSettings _dbSettings = dbSettingsMonitor.Get(
        DatabaseSettingsInstances.SQL
    );

    internal const string QueryCommand = "SP_SEL_LASTPROCESS";
    internal const string PopulateCommand = "SP_INS_ONBASECOREDATA";

    public async Task<IEnumerable<MigrationsBase>> List(
        ViewMigrationsDto dto,
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

    public async Task<MigrationsBase> Create(
        CreateMigrationsDto dto,
        CancellationToken cancellationToken = default
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

    public async Task<MigrationsBase> View(
        ViewMigrationsDto dto,
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
}
