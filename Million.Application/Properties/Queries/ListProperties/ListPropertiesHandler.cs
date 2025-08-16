using MediatR;
using Million.Application.Abstractions.Persistence;
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
    /// Handler que delega la consulta paginada al repositorio de lectura.
    /// </summary>
    public sealed class ListPropertiesQueryHandler
        : IRequestHandler<ListPropertiesQuery, Result<PagedList<PropertyListItemDTO>>>
    {
        private readonly IPropertyReadRepository _readRepo;

        public ListPropertiesQueryHandler(IPropertyReadRepository readRepo) => _readRepo = readRepo;

        public async Task<Result<PagedList<PropertyListItemDTO>>> Handle(ListPropertiesQuery q, CancellationToken ct)
        {
            var page = q.Page <= 0 ? 1 : q.Page;
            var pageSize = q.PageSize <= 0 ? 20 : q.PageSize;
            var orderBy = string.IsNullOrWhiteSpace(q.OrderBy) ? "price_desc" : q.OrderBy;

            var paged = await _readRepo.ListAsync(
                q.City, q.State,
                q.MinPrice, q.MaxPrice,
                q.MinYear, q.MaxYear,
                q.Rooms,
                page, pageSize,
                orderBy,
                ct);

            return Result.Success(paged);
        }
    }
}
