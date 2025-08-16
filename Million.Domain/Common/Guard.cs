using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Common
{
    /// <summary>
    /// Conjunto de reglas reutilizables para validaciones
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Valida que una cadena no sea nula ni esté en blanco.
        /// </summary>
        public static void AgainstNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{paramName} es requerido.");
        }

        /// <summary>
        /// Valida que un entero esté dentro del rango permitido [min, max].
        /// </summary>
        public static void AgainstOutOfRange(int value, int min, int max, string paramName)
        {
            if (value < min || value > max)
                throw new DomainException($"{paramName} debe estar entre {min} y {max}.");
        }

        /// <summary>
        /// Valida que un decimal sea estrictamente mayor a cero.
        /// </summary>
        public static void AgainstNonPositive(decimal value, string paramName)
        {
            if (value <= 0)
                throw new DomainException($"{paramName} debe ser mayor a 0.");
        }
    }
}
