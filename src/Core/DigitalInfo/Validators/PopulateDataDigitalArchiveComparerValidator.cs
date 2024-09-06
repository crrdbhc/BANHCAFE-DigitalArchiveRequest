using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Models;
using FluentValidation;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.DigitalInfo.Validators;

public class PopulateDataDigitalArchiveComparerValidator
    : AbstractValidator<PopulateDataDigitalArchive>
{
    public PopulateDataDigitalArchiveComparerValidator() { }
}
