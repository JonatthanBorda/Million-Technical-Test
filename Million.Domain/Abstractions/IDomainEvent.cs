using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Abstractions
{
    /// <summary>
    /// Contrato mínimo para un evento de dominio.
    /// Un evento representa un hecho que ocurrió en el negocio (cambio de precio).
    /// No depende de frameworks (MediatR o EF), para mantener el dominio puro.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Momento en el que ocurrió el evento (trazabilidad).
        /// </summary>
        DateTime OccurredOn { get; }
    }
}
