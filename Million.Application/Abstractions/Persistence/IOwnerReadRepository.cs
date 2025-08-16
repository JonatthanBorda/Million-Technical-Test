using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions.Persistence
{
    /// <summary>
    /// Repositorio de lectura para Owners.
    /// </summary>
    public interface IOwnerReadRepository
    {
        Task<IReadOnlyList<OwnerDTO>> ListWithPropertiesAsync(CancellationToken ct);
        Task<IReadOnlyList<OwnerBasicDTO>> ListAllAsync(CancellationToken ct);
        Task<bool> ExistsAsync(Guid ownerId, CancellationToken ct);
    }
}
