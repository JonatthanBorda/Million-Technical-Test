using Million.Domain.Abstractions;
using Million.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Properties
{
    /// <summary>
    /// Entidad que representa una imagen asociada a una propiedad.
    /// </summary>
    public sealed class PropertyImage : Entity
    {
        /// <summary>
        /// Id de la propiedad de la imagen.
        /// </summary>
        public Guid PropertyId { get; private set; }

        /// <summary>
        /// Ruta o URL del archivo de imagen.
        /// </summary>
        public string File { get; private set; } = default!;

        /// <summary>
        /// Indica si la imagen está habilitada para mostrarse.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Constructor privado para EF Core.
        /// </summary>
        private PropertyImage() { }

        /// <summary>
        /// Crea una imagen válida asociada a una propiedad.
        /// </summary>
        internal PropertyImage(Guid propertyId, string file, bool enabled)
        {
            Guard.AgainstNullOrWhiteSpace(file, nameof(file));
            PropertyId = propertyId;
            File = file;
            Enabled = enabled;
        }

        /// <summary>
        /// Habilita la imagen.
        /// </summary>
        public void Enable() => Enabled = true;

        /// <summary>
        /// Deshabilita la imagen.
        /// </summary>
        public void Disable() => Enabled = false;
    }
}
