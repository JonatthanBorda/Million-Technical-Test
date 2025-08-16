using MediatR;
using Million.Application.Common;
using Million.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Properties.Commands.ChangePropertyPrice
{
    /// <summary>
    /// Cambia el precio de una propiedad (registra traza y emite evento de dominio).
    /// </summary>
    /// <param name="PropertyId"></param>
    /// <param name="NewAmount"></param>
    /// <param name="Currency"></param>
    /// <param name="DateSale"></param>
    /// <param name="Tax"></param>
    public sealed record ChangePropertyPriceCommand(
        Guid PropertyId,
        decimal NewAmount,
        string Currency,
        DateOnly? DateSale,
        decimal? Tax
    ) : IRequest<Result<PropertyDTO>>;
}
