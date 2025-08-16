using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Million.Application.Common;
using Million.Application.DTOs;
using Million.Application.Properties.Commands.AddPropertyImage;
using Million.Application.Properties.Commands.ChangePropertyPrice;
using Million.Application.Properties.Commands.CreateProperty;
using Million.Application.Properties.Commands.UpdateProperty;
using Million.Application.Properties.Queries.ListProperties;
using Million.WebApi.Contracts.Requests;
using Million.WebApi.Contracts.Responses;
using Million.WebApi.Services.Images;
using Million.Domain.Common;

namespace Million.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/properties")]
    [ApiVersion("1.0")]
    [Authorize]
    public sealed class PropertiesController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IImageStorage _images;

        public PropertiesController(ISender mediator, IImageStorage images)
        {
            _mediator = mediator;
            _images = images;
        }

        /// <summary>
        /// Crea una nueva propiedad.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PropertyDTO>>> Create([FromBody] CreatePropertyRequest req, CancellationToken ct)
        {
            var cmd = new CreatePropertyCommand(
                req.Name, req.Street, req.City, req.State, req.Zip,
                req.Price, req.Currency, req.CodeInternal, req.Year, req.Rooms, req.OwnerId);

            var result = await _mediator.Send(cmd, ct);
            if (!result.IsSuccess)
                return ToProblem(result.Error?.Code, result.Error?.Message);

            return CreatedAtAction(nameof(GetList), new { version = "1.0" }, ApiResponse<PropertyDTO>.Ok(result.Value!));
        }

        /// <summary>
        /// Agrega una imagen (multipart/form-data) a una propiedad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/images")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5 * 1024 * 1024)] //5 MB
        public async Task<ActionResult<ApiResponse<PropertyDTO>>> AddImage([FromRoute] Guid id, [FromForm] AddPropertyImageForm form, CancellationToken ct)
        {
            string? path = null;

            try
            {
                //1. Guarda físicamente el archivo:
                path = await _images.SaveAsync(form.File, ct);

                //2. Envía el comando para registrar la imagen en BD:
                var cmd = new AddPropertyImageCommand(id, path, form.Enabled);
                var result = await _mediator.Send(cmd, ct);

                //3. Si el handler devolvió error de dominio:
                if (!result.IsSuccess)
                    return ToProblem(result.Error?.Code, result.Error?.Message);

                return Ok(ApiResponse<PropertyDTO>.Ok(result.Value!));
            }
            catch
            {
                //Si falló la BD elimina el archivo:
                if (!string.IsNullOrWhiteSpace(path))
                {
                    try { await _images.DeleteAsync(path, ct); } catch { }
                }

                //Lo manejará tu middleware:
                throw;
            }
        }

        /// <summary>
        /// Cambia el precio de una propiedad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/price")]
        public async Task<ActionResult<ApiResponse<PropertyDTO>>> ChangePrice([FromRoute] Guid id, [FromBody] ChangePriceRequest req, CancellationToken ct)
        {
            var cmd = new ChangePropertyPriceCommand(id, req.NewAmount, req.Currency, req.DateSale, req.Tax);

            var result = await _mediator.Send(cmd, ct);
            if (!result.IsSuccess)
                return ToProblem(result.Error?.Code, result.Error?.Message);

            return Ok(ApiResponse<PropertyDTO>.Ok(result.Value!));
        }

        /// <summary>
        /// Actualiza la información de una propiedad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PropertyDTO>>> Update([FromRoute] Guid id, [FromBody] UpdatePropertyRequest req, CancellationToken ct)
        {
            var cmd = new UpdatePropertyCommand(id, req.Name, req.Street, req.City, req.State, req.Zip, req.Year, req.Rooms);

            var result = await _mediator.Send(cmd, ct);
            if (!result.IsSuccess)
                return ToProblem(result.Error?.Code, result.Error?.Message);

            return Ok(ApiResponse<PropertyDTO>.Ok(result.Value!));
        }

        /// <summary>
        /// Lista propiedades con filtros, paginación y orden.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="minYear"></param>
        /// <param name="maxYear"></param>
        /// <param name="rooms"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<ActionResult<ApiResponse<PagedList<PropertyListItemDTO>>>> GetList(
            [FromQuery] string? city, [FromQuery] string? state,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] int? minYear, [FromQuery] int? maxYear,
            [FromQuery] int? rooms,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? orderBy = "price_desc",
            CancellationToken ct = default)
        {
            var q = new ListPropertiesQuery(city, state, minPrice, maxPrice, minYear, maxYear, rooms, page, pageSize, orderBy);
            var result = await _mediator.Send(q, ct);

            return Ok(ApiResponse<PagedList<PropertyListItemDTO>>.Ok(result.Value!, new { page, pageSize }));
        }

        /// <summary>
        /// Mapea las claves de error de Application/Domain a ProblemDetails con el status HTTP correcto.
        /// </summary>
        private ActionResult ToProblem(string? code, string? message)
        {
            code ??= "domain.unknown";
            message ??= "Se produjo un error de dominio.";

            return code switch
            {
                //Propietario no encontrado:
                "owner.not_found" => Problem(
                    type: "https://million/errors/not-found",
                    title: "Propietario inexistente",
                    detail: message,
                    statusCode: StatusCodes.Status404NotFound),

                //Propiedad no encontrada:
                "property.not_found" => Problem(
                    type: "https://million/errors/not-found",
                    title: "No encontrado",
                    detail: message,
                    statusCode: StatusCodes.Status404NotFound),

                //Duplicado global de CodeInternal:
                "property.duplicate_codeinternal" => Problem(
                    type: "https://million/errors/conflict",
                    title: "Datos duplicados",
                    detail: message,
                    statusCode: StatusCodes.Status409Conflict),

                //Cualquier otro error de dominio/negocio:
                _ => Problem(
                    type: "https://million/errors/domain",
                    title: "Error de dominio",
                    detail: message,
                    statusCode: StatusCodes.Status422UnprocessableEntity)
            };
        }
    }
}
