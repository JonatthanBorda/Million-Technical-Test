using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.ChangePropertyPrice
{
    /// <summary>
    /// Validaciones para cambio de precio.
    /// </summary>
    public sealed class ChangePropertyPriceValidator : AbstractValidator<ChangePropertyPriceCommand>
    {
        public ChangePropertyPriceValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.NewAmount).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.Tax).GreaterThanOrEqualTo(0).When(x => x.Tax.HasValue);
        }
    }
}
