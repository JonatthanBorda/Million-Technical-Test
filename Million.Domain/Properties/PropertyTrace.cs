using Million.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Properties
{
    /// <summary>
    /// Entidad que registra un histórico de precio/venta de la propiedad.
    /// Se agrega cuando el precio cambia para dejar evidencia y auditoría.
    /// </summary>
    public sealed class PropertyTrace : Entity
    {
        /// <summary>Id de la propiedad relacionada.</summary>
        public Guid PropertyId { get; private set; }

        /// <summary>Fecha de la venta o del cambio de precio.</summary>
        public DateOnly DateSale { get; private set; }

        /// <summary>Nombre de la propiedad al momento del registro.</summary>
        public string Name { get; private set; } = default!;

        /// <summary>Valor/precio del registro.</summary>
        public decimal Value { get; private set; }

        /// <summary>Impuesto aplicado (cuando corresponde).</summary>
        public decimal Tax { get; private set; }

        /// <summary>Constructor privado para EF Core.</summary>
        private PropertyTrace() { }

        /// <summary>
        /// Crea un registro de traza válido.
        /// </summary>
        internal PropertyTrace(Guid propertyId, DateOnly dateSale, string name, decimal value, decimal tax)
        {
            PropertyId = propertyId;
            DateSale = dateSale;
            Name = name;
            Value = value;
            Tax = tax;
        }
    }
}
