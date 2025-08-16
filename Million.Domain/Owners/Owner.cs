using Million.Domain.Abstractions;
using Million.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Owners
{
    /// <summary>
    /// Dueño de una o varias propiedades.
    /// </summary>
    public sealed class Owner : AggregateRoot
    {
        /// <summary>
        /// Nombre completo del propietario.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Dirección postal del propietario.
        /// </summary>
        public string? Address { get; private set; }

        /// <summary>
        /// URL o ruta al recurso de foto del propietario.
        /// </summary>
        public string? Photo { get; private set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        public DateOnly? Birthday { get; private set; }

        /// <summary>
        /// Constructor privado para EF Core.
        /// </summary>
        private Owner() { Name = string.Empty; }

        /// <summary>
        /// Crea un nuevo propietario realizando validaciones establecidas.
        /// </summary>
        public Owner(string name, string? address, string? photo, DateOnly? birthday)
        {
            Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Name = name;
            Address = address;
            Photo = photo;
            Birthday = birthday;
        }

        /// <summary>
        /// Cambia el nombre del propietario aplicando validación.
        /// </summary>
        public void Rename(string newName)
        {
            Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
            Name = newName;
        }
    }
}
