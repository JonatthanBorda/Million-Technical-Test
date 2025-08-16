using System.ComponentModel.DataAnnotations;

namespace Million.WebApi.Contracts.Requests
{
    /// <summary>
    /// Modelo para subida de imagen (multipart/form-data).
    /// </summary>
    public sealed class AddPropertyImageForm
    {
        /// <summary>
        /// Archivo de imagen (jpg/png/webp) máx 5MB.
        /// </summary>
        [Required]
        public IFormFile File { get; set; } = default!;
        /// <summary>
        /// Sí la imagen queda habilitada (default: true).
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
