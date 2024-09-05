using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Models;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Queries;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Mapper;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ListDigitalArchiveComparerQuery, ViewDigitalArchiveComparerDto>();
    }
}
