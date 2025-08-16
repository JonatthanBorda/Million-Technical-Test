using Microsoft.EntityFrameworkCore;
using Million.Application.Abstractions.Persistence;
using Million.Domain.Properties;

namespace Million.Infrastructure.Persistence.Repositories
{
    internal sealed class PropertyRepository : IPropertyRepository
    {
        private readonly MillionDbContext _db;
        public PropertyRepository(MillionDbContext db) => _db = db;
        public void Add(Property entity) => _db.Properties.Add(entity);
        public void Update(Property entity) => _db.Entry(entity).State = EntityState.Modified;
        public void Remove(Property entity) => _db.Properties.Remove(entity);
        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct)
            => await _db.Properties
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
        public void AddTrace(PropertyTrace trace) => _db.Set<PropertyTrace>().Add(trace);
    }
}
