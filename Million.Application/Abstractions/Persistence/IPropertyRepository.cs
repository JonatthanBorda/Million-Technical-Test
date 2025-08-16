using Million.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions.Persistence
{
    /// <summary>
    /// Repositorio de escritura para Property.
    /// </summary>
    public interface IPropertyRepository
    {
        void Add(Property entity);
        void Update(Property entity);
        void Remove(Property entity);
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct);
        void AddTrace(PropertyTrace trace);
    }
}
