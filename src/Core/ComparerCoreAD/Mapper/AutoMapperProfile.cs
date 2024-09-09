using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.ComparerCoreAD.Models;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.ComparerCoreAD.Queries;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.ComparerCoreAD.Mapper;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ListDigitalArchiveComparerQuery, ViewDigitalArchiveComparerDto>();
        CreateMap<PopulateDataDigitalArchive, PopulateDataDigitalArchiveComparerDto>();
    }
}
