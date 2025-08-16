using MediatR;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.CreateProperty
{
    /// <summary>
    /// Crea una propiedad nueva.
    /// </summary>
    public sealed record CreatePropertyCommand(
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
        Guid OwnerId
    ) : IRequest<Result<PropertyDTO>>;
}
