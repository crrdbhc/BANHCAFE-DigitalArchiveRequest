using AutoMapper;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Extensions;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Ports;
using MediatR;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Queries;

public sealed class ListDigitalArchiveComparerQuery
    : BaseQuery,
        IRequest<ApiResponse<IEnumerable<DigitalArchiveComparerBase>>>
{
    public int? AgencyNum { get; set; }
    public string? ProductNum { get; set; }
    public string? ClientNum { get; set; }
}

public sealed class DigitalArchiveComparerHandler(
    IDigitalArchiveComparerRepository repository,
    IMapper mapper,
    IUserContext userContext
)
    : IRequestHandler<
        ListDigitalArchiveComparerQuery,
        ApiResponse<IEnumerable<DigitalArchiveComparerBase>>
    >
{
    public async Task<ApiResponse<IEnumerable<DigitalArchiveComparerBase>>> Handle(
        ListDigitalArchiveComparerQuery request,
        CancellationToken cancellationToken
    )
    {
        var apiResponse = new ApiResponse<IEnumerable<DigitalArchiveComparerBase>>();

        var filtersDto = mapper.Map<ViewDigitalArchiveComparerDto>(request);
        filtersDto.TerminalId = userContext.User.GetTerminalId();

        var results = await repository.List(filtersDto, cancellationToken);

        if (results.Any())
        {
            BaseQueryResponseDto pagination = results.FirstOrDefault();
            if (pagination is null || !pagination.Success)
            {
                apiResponse.AddError(pagination?.Message ?? "Ocurrio un error.");
                return apiResponse;
            }

            apiResponse.AddPagination(pagination);

            apiResponse.Data = results;
        }

        return apiResponse;
    }
}
