using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BANHCAFE.Cross.DBConnection;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Extensions;
using Banhcafe.Microservices.ComparerCoreVsAD.Infrastructure.Common.Ports;
using Grpc.Core.Logging;
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
}
