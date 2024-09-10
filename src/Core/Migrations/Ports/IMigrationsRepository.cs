using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Ports
{
    public interface IMigrationsRepository : IGenericRepository<MigrationsBase, ViewMigrationsDto>
    {
        Task<MigrationsBase> Create(CreateMigrationsDto dto, CancellationToken cancellationToken);
        Task<IEnumerable<MigrationsBase>> List(
            ViewMigrationsDto filtersDto,
            CancellationToken cancellationToken
        );
    }
}
