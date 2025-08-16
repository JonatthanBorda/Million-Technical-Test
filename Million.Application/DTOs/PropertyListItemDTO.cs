using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// DTO de listados/paginación para responses de Queries.
    /// </summary>
    public sealed record PropertyListItemDTO(
        Guid Id,
        string Name,
        string Street,
        string City,
        string State,
        string CodeInternal,
        decimal Price,
        string Currency,
        int Year,
        int? Rooms,
        IReadOnlyCollection<PropertyImageItemDTO> Images,
        IReadOnlyCollection<PropertyTraceItemDTO> Traces);
}
