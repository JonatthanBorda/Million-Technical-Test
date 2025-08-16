using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// DTO liviano para exponer el histórico de cambios de precio (traces) de una propiedad.
    /// </summary>
    public sealed record PropertyTraceItemDTO(
        Guid Id,
        DateOnly DateSale,
        string Name,
        decimal Value,
        decimal Tax
    );
}
