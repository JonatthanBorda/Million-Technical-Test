using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.DTOs
{
    /// <summary>
    /// DTO de propiedad para responses de Commands.
    /// </summary>
    public sealed record PropertyDTO(
        Guid Id,
        string Name,
        string Street,
        string City,
        string State,
        string Zip,
        decimal Price,
        string Currency,
        string CodeInternal,
        int Year,
        int? Rooms,
        Guid OwnerId,
        IReadOnlyCollection<PropertyImageItemDTO> Images
    );
}
