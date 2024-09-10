using AutoMapper;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Queries;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Mapper;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ListMigrationsQuery, ViewMigrationsDto>();
    }
}
