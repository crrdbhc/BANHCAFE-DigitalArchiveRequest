using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;

public interface IGenericRepository<TResponse, TFilterRequest>
{
    Task<IEnumerable<TResponse>> List(
        TFilterRequest dto = default,
        CancellationToken cancellationToken = default
    );
}
