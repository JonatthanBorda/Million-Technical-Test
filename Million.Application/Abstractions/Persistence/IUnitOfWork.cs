using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions.Persistence
{
    /// <summary>
    /// Unidad de trabajo: coordina transacciones entre múltiples repositorios.
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
