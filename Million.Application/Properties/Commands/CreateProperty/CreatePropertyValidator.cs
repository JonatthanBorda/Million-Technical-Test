using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.CreateProperty
{
    /// <summary>
    /// Validaciones de entrada para crear propiedad.
    /// </summary>
    public sealed class CreatePropertyValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
            RuleFor(x => x.City).NotEmpty().MaximumLength(120);
            RuleFor(x => x.State).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Zip).NotEmpty().MaximumLength(10);

            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);

            RuleFor(x => x.CodeInternal).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Year).InclusiveBetween(1800, DateTime.UtcNow.Year + 1);
            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}
