using MediatR;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Queries.ListProperties
{
    /// <summary>
    /// Lista propiedades filtrando por ciudad/estado, rango de precio, año, habitaciones.
    /// Aplica paginación y orden.
    /// </summary>
    public sealed record ListPropertiesQuery(
        string? City,
        string? State,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? MinYear,
        int? MaxYear,
        int? Rooms,
        int Page = 1,
        int PageSize = 20,
        string? OrderBy = "price_desc" //Ordena por precio de mayor a menor
    ) : IRequest<Result<PagedList<PropertyListItemDTO>>>;
}
