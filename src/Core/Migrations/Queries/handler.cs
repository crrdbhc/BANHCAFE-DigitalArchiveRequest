using AutoMapper;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Extensions;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Ports;
using MediatR;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Queries;

public sealed class ListMigrationsQuery
    : BaseQuery,
        IRequest<ApiResponse<IEnumerable<MigrationsBase>>> { }

public sealed class MigrationsHandler(
    IMigrationsRepository repository,
    IMapper mapper,
    IUserContext userContext
) : IRequestHandler<ListMigrationsQuery, ApiResponse<IEnumerable<MigrationsBase>>>
{
    public async Task<ApiResponse<IEnumerable<MigrationsBase>>> Handle(
        ListMigrationsQuery request,
        CancellationToken cancellationToken
    )
    {
        var apiResponse = new ApiResponse<IEnumerable<MigrationsBase>>();

        var filtersDto = mapper.Map<ViewMigrationsDto>(request);
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
