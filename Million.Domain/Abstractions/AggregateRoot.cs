using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Abstractions
{
    /// <summary>
    /// Marca una raíz de agregado (punto de consistencia transaccional).
    /// Todas las modificaciones a las entidades internas deben orquestarse desde aquí.
    /// </summary>
    public abstract class AggregateRoot : Entity { }
}
