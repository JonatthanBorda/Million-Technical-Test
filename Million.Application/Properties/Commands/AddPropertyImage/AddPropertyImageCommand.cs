using MediatR;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.AddPropertyImage
{
    /// <summary>
    /// Agrega una imagen a la propiedad indicada.
    /// </summary>
    /// <param name="PropertyId"></param>
    /// <param name="File"></param>
    /// <param name="Enabled"></param>
    public sealed record AddPropertyImageCommand(
        Guid PropertyId,
        string File,
        bool Enabled = true
    ) : IRequest<Result<PropertyDTO>>;
}
