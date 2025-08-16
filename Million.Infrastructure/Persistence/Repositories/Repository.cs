using Microsoft.EntityFrameworkCore;
using Million.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementación genérica de repositorio.
    /// </summary>
    public sealed class Repository<T> : IRepository<T> where T : class
    {
        private readonly MillionDbContext _db;
        private readonly DbSet<T> _set;

        public Repository(MillionDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
            => await _set.FindAsync([id], ct);

        public async Task AddAsync(T entity, CancellationToken ct)
            => await _set.AddAsync(entity, ct);

        public void Update(T entity) => _set.Update(entity);

        public void Remove(T entity) => _set.Remove(entity);
    }
}
