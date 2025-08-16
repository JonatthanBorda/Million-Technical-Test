using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.UpdateProperty
{
    /// <summary>
    /// Validaciones para actualizar propiedad.
    /// </summary>
    public sealed class UpdatePropertyValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
            RuleFor(x => x.City).NotEmpty().MaximumLength(120);
            RuleFor(x => x.State).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Zip).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Year).InclusiveBetween(1800, DateTime.UtcNow.Year + 1);
            RuleFor(x => x.Rooms).GreaterThanOrEqualTo(0).When(x => x.Rooms.HasValue);
        }
    }
}
