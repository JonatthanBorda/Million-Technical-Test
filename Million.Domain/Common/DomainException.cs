using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Common
{
    /// <summary>
    /// Excepción de negocio para violaciones de invariantes del dominio.
    /// </summary>
    public sealed class DomainException : Exception
    {
        /// <summary>
        /// Crea una nueva excepción de dominio con el mensaje especificado.
        /// </summary>
        public DomainException(string message) : base(message) { }
    }
}
