using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Common
{
    /// <summary>
    /// Representa un error de negocio o aplicación con un código estable y un mensaje legible.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// Código estable del error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Mensaje descriptivo para clientes/operadores.
        /// </summary>
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString() => $"{Code}: {Message}";
    }
}
