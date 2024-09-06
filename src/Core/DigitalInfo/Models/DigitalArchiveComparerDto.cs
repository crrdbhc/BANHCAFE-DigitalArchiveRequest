using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Models;

public class DigitalArchiveComparerBase : BaseQueryResponseDto
{
    public int? RowId { get; set; }
    public int? Size { get; set; }
    public int? TotalRows { get; set; }
    public int? TotalCount { get; set; }
    public int? AgencyNum { get; set; }
    public string? ClientNum { get; set; }
    public string? ClientCoreName { get; set; }
    public string? ClientOnBaseName { get; set; }
    public string? ProductNum { get; set; }
    public string? ProductType { get; set; }
    public string? ProductState { get; set; }
    public string? OpeningDate { get; set; }
    public string? ADReceptionDate { get; set; }
    public string? ADReceptionStatus { get; set; }
    public string? ProductInCore { get; set; }
    public string? ProductInOnBase { get; set; }
    public string? Match { get; set; }
    public string? Response { get; set; }
}

public class ViewDigitalArchiveComparerDto : IBaseQueryDto
{
    public int? AgencyNum { get; set; }
    public string? ProductNum { get; set; }
    public string? ClientNum { get; set; }
    public int? TerminalId { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
}

public class PopulateDataDigitalArchive { }

public class PopulateDataDigitalArchiveComparerDto { }
