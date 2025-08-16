using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Common
{
    /// <summary>
    /// Resultado paginado para Queries.
    /// </summary>
    public sealed record PagedList<T>(
        IReadOnlyList<T> Items,
        int Page,
        int PageSize,
        long TotalCount);
}
