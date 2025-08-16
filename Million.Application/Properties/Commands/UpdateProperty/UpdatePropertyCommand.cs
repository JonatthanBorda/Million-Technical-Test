using MediatR;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.UpdateProperty
{
    /// <summary>
    /// Actualiza datos de una propiedad.
    /// </summary>
    /// <param name="PropertyId"></param>
    /// <param name="Name"></param>
    /// <param name="Street"></param>
    /// <param name="City"></param>
    /// <param name="State"></param>
    /// <param name="Zip"></param>
    /// <param name="Year"></param>
    /// <param name="Rooms"></param>
    public sealed record UpdatePropertyCommand(
        Guid PropertyId,
        string? Name,
        string Street,
        string City,
        string State,
        string Zip,
        int Year,
        int? Rooms
    ) : IRequest<Result<PropertyDTO>>;
}
