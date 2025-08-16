using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Million.Application.Abstractions.Persistence;
using Million.Application.DTOs;

namespace Million.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositorio de lectura para Owners.
    /// Devuelve solo owners con al menos una Property relacionada (1:N).
    /// </summary>
    public sealed class OwnerReadRepository : IOwnerReadRepository
    {
        private readonly MillionDbContext _db;

        public OwnerReadRepository(MillionDbContext db) => _db = db;

        public async Task<IReadOnlyList<OwnerDTO>> ListWithPropertiesAsync(CancellationToken ct)
        {
            var query =
            from o in _db.Owners.AsNoTracking()
            join p in _db.Properties.AsNoTracking() on o.Id equals p.OwnerId into gj
            where gj.Any()
            select new OwnerDTO(
                o.Id,
                o.Name,
                gj.Count(),
                gj.Select(x => new OwnerPropertyItemDTO(
                        x.Id,
                        x.Name,
                        x.CodeInternal
                    ))
                  .ToList()
            );

            return await query.ToListAsync(ct);
        }

        public async Task<IReadOnlyList<OwnerBasicDTO>> ListAllAsync(CancellationToken ct)
        {
            return await _db.Owners
                .AsNoTracking()
                .OrderBy(o => o.Name)
                .Select(o => new OwnerBasicDTO(o.Id, o.Name))
                .ToListAsync(ct);
        }

        public Task<bool> ExistsAsync(Guid ownerId, CancellationToken ct) =>
            _db.Owners.AsNoTracking().AnyAsync(o => o.Id == ownerId, ct);
    }
}
