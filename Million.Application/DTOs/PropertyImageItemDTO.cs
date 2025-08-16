using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// DTO para imágenes de la propiedad.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="File"></param>
    /// <param name="Enabled"></param>
    public sealed record PropertyImageItemDTO(
        Guid Id,
        string File,
        bool Enabled
    );
}
