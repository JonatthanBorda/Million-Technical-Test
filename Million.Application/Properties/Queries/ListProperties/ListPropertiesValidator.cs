using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Queries.ListProperties
{
    /// <summary>
    /// Validaciones de entrada para el listado.
    /// </summary>
    public sealed class ListPropertiesValidator : AbstractValidator<ListPropertiesQuery>
    {
        public ListPropertiesValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200);

            RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue);
            RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue);
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.MinYear).GreaterThanOrEqualTo(1800).When(x => x.MinYear.HasValue);
            RuleFor(x => x.MaxYear).LessThanOrEqualTo(DateTime.UtcNow.Year + 1).When(x => x.MaxYear.HasValue);
            RuleFor(x => x.MaxYear)
                .GreaterThanOrEqualTo(x => x.MinYear!.Value)
                .When(x => x.MinYear.HasValue && x.MaxYear.HasValue);

            RuleFor(x => x.OrderBy)
                .Must(v => v is null || new[] {
                "price_asc","price_desc","year_asc","year_desc","name_asc","name_desc"
                }.Contains(v))
                .WithMessage("OrderBy inválido. Use: price_asc|price_desc|year_asc|year_desc|name_asc|name_desc");
        }
    }
}
