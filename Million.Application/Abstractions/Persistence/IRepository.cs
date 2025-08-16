using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions.Persistence
{
    /// <summary>
    /// Repositorio genérico para operaciones de escritura/lectura básica sobre agregados.
    /// Se usa en Commands (CQRS).
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        void Update(T entity);
        void Remove(T entity);
    }
}
