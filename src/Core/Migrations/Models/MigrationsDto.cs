using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;

public class MigrationsBase : BaseQueryResponseDto
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Status { get; set; }
    public bool? WithErrors { get; set; }
}

public class CreateMigrations { }

public class CreateMigrationsDto : CreateMigrations { }

public class ViewMigrationsDto : IBaseQueryDto
{
    public int? TerminalId { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
}
