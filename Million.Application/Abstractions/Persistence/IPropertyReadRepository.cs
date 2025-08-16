using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions.Persistence
{
    /// <summary>
    /// Repositorio de lectura para Queries.
    /// Retorna DTOs paginados.
    /// </summary>
    public interface IPropertyReadRepository
    {
        Task<PagedList<PropertyListItemDTO>> ListAsync(
            string? city, string? state,
            decimal? minPrice, decimal? maxPrice,
            int? minYear, int? maxYear,
            int? rooms,
            int page, int pageSize,
            string? orderBy,
            CancellationToken ct);

        /// <summary>
        /// Verifica si existe un CodeInternal en una propiedad existente.
        /// </summary>
        Task<bool> ExistsCodeInternalAsync(string codeInternal, CancellationToken ct);
    }
}
