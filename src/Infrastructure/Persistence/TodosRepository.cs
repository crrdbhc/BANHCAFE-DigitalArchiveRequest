using BANHCAFE.Cross.DBConnection;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Data;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Ports;
using Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure.Common.Extensions;
using Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure.Common.Ports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Infrastructure.Persistence;

public sealed class TodosRepository : ITodosRepository
{
    private readonly ILogger<TodosRepository> _logger;
    private readonly DatabaseSettings _dbSettings;
    private readonly ISqlDbConnectionApiExtensions<object, IEnumerable<TodoDto>> _api;

    public TodosRepository(
        ILogger<TodosRepository> logger,
        ISqlDbConnectionApiExtensions<object, IEnumerable<TodoDto>> api,
        IOptionsSnapshot<DatabaseSettings> dbSettings
    )
    {
        _logger = logger;
        _dbSettings = dbSettings.Value;
        _api = api;
    }

    public async Task<TodoDto> CreateAsync(TodoDto dto, CancellationToken cancellationToken)
    {
        var request = new SqlDbApiRequest<object>
        {
            Scheme = _dbSettings.SchemeName,
            Database = _dbSettings.DatabaseName,
            Parameters = new { dto.Name },
            StoredProcedure = "SP_INS_TODO"
        };

        var response = await _api.Process(_logger, request, cancellationToken);

        return response.FirstOrDefault()!;
    }
}
