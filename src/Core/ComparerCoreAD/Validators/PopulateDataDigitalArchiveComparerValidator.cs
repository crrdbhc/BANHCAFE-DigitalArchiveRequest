using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Models;
using FluentValidation;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.ComparerCoreAD.Validators;

public class PopulateDataDigitalArchiveComparerValidator
    : AbstractValidator<PopulateDataDigitalArchive>
{
    public PopulateDataDigitalArchiveComparerValidator() { }
}
