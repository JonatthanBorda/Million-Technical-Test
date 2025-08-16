using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// Propiedad para mostrar dentro del Owner.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="CodeInternal"></param>
    public sealed record OwnerPropertyItemDTO(
        Guid Id,
        string Name,
        string CodeInternal);

    /// <summary>
    /// DTO de Owner con sus propiedades asociadas.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="PropertiesCount"></param>
    /// <param name="Properties"></param>
    public sealed record OwnerDTO(
        Guid Id,
        string Name,
        int PropertiesCount,
        IReadOnlyCollection<OwnerPropertyItemDTO> Properties);

    /// <summary>
    /// DTO simple para listar todos los Owners.
    /// </summary>
    public sealed record OwnerBasicDTO(
        Guid Id,
        string Name
    );
}
