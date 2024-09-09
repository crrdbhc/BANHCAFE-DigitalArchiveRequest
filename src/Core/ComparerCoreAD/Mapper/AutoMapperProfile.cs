using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Queries;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Mapper;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ListDigitalArchiveComparerQuery, ViewDigitalArchiveComparerDto>();
        CreateMap<PopulateDataDigitalArchive, PopulateDataDigitalArchiveComparerDto>();
    }
}
