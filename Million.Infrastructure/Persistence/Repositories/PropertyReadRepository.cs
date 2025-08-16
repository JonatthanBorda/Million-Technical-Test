using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositorio para lectura: filtra/ordena/pagina y proyecta a DTO en el servidor.
    /// </summary>
    public sealed class PropertyReadRepository : IPropertyReadRepository
    {
        private readonly MillionDbContext _db;
        private readonly IMapper _mapper;

        public PropertyReadRepository(MillionDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedList<PropertyListItemDTO>> ListAsync(
            string? city, string? state,
            decimal? minPrice, decimal? maxPrice,
            int? minYear, int? maxYear,
            int? rooms,
            int page, int pageSize,
            string? orderBy,
            CancellationToken ct)
        {
            var q = _db.Properties.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(city)) q = q.Where(p => p.Address.City == city);
            if (!string.IsNullOrWhiteSpace(state)) q = q.Where(p => p.Address.State == state);
            if (minPrice.HasValue) q = q.Where(p => p.Price.Amount >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.Price.Amount <= maxPrice.Value);
            if (minYear.HasValue) q = q.Where(p => p.Year >= minYear.Value);
            if (maxYear.HasValue) q = q.Where(p => p.Year <= maxYear.Value);
            if (rooms.HasValue) q = q.Where(p => p.Rooms == rooms.Value);

            q = orderBy switch
            {
                "price_asc" => q.OrderBy(p => (double)p.Price.Amount),
                "price_desc" => q.OrderByDescending(p => (double)p.Price.Amount),
                "year_asc" => q.OrderBy(p => p.Year),
                "year_desc" => q.OrderByDescending(p => p.Year),
                "name_asc" => q.OrderBy(p => p.Name),
                "name_desc" => q.OrderByDescending(p => p.Name),
                _ => q.OrderByDescending(p => (double)p.Price.Amount)
            };

            var total = await q.LongCountAsync(ct);

            //Proyección del lado del servidor:
            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PropertyListItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new PagedList<PropertyListItemDTO>(items, page, pageSize, total);
        }
        public Task<bool> ExistsCodeInternalAsync(string codeInternal, CancellationToken ct)
        {
            codeInternal = (codeInternal ?? string.Empty).Trim();
            return _db.Properties.AsNoTracking().AnyAsync(p => p.CodeInternal == codeInternal, ct);
        }
    }
}
