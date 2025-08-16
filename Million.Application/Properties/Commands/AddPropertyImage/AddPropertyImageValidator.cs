using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.AddPropertyImage
{
    /// <summary>
    /// Validaciones de entrada para agregar imagen.
    /// </summary>
    public sealed class AddPropertyImageValidator : AbstractValidator<AddPropertyImageCommand>
    {
        public AddPropertyImageValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.File)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
