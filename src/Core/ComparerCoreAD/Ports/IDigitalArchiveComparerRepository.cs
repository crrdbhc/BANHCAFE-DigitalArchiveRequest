using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Ports;

public interface IDigitalArchiveComparerRepository
    : IGenericRepository<DigitalArchiveComparerBase, ViewDigitalArchiveComparerDto>
{
    Task<IEnumerable<DigitalArchiveComparerBase>> List(
        ViewDigitalArchiveComparerDto filtersDto,
        CancellationToken cancellationToken
    );

    Task<DigitalArchiveComparerBase> Populate(
        PopulateDataDigitalArchiveComparerDto dto,
        CancellationToken cancellationToken
    );
}
