using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Abstractions
{
    /// <summary>
    /// Clase base para entidades del dominio.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Identificador único de la entidad (GUID).
        /// </summary>
        public Guid Id { get; protected set; } = Guid.NewGuid();

        //Lista interna de eventos de dominio generados por esta entidad/agregado.
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Eventos de dominio pendientes de despachar.
        /// </summary>
        public ReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Agrega un evento de dominio a la cola local del agregado.
        /// Infraestructura o Application serán responsables de despacharlo.
        /// </summary>
        protected void Raise(IDomainEvent @event) => _domainEvents.Add(@event);

        /// <summary>
        /// Limpia la lista de eventos.
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
