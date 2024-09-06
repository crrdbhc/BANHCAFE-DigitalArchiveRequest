using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Models;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Ports;

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
