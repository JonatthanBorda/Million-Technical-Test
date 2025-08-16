using Million.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Events
{
    /// <summary>
    /// Evento de dominio que se emite cuando el precio de una propiedad cambia.
    /// </summary>
    /// <param name="PropertyId">Id de la propiedad cuya información cambió.</param>
    /// <param name="OldPrice">Precio anterior.</param>
    /// <param name="NewPrice">Nuevo precio.</param>
    public sealed record PriceChangedDomainEvent(Guid PropertyId, decimal OldPrice, decimal NewPrice)
        : IDomainEvent
    {
        /// <summary>
        /// Fecha en la que se realiza el cambio.
        /// </summary>
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
