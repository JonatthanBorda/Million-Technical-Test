using Million.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Unidad de trabajo sobre el DbContext.
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly MillionDbContext _db;

        public UnitOfWork(MillionDbContext db) => _db = db;

        public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
