using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using Million.WebApi.Contracts.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Million.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/owners")]
    [ApiVersion("1.0")]
    public sealed class OwnersController : ControllerBase
    {
        private readonly IOwnerReadRepository _owners;

        public OwnersController(IOwnerReadRepository owners) => _owners = owners;

        /// <summary>
        /// Lista los owners que tienen al menos una propiedad asociada,
        /// incluyendo la cantidad y un listado ligero de sus propiedades.
        /// </summary>
        [HttpGet("withProperty")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<OwnerDTO>>>> Get(CancellationToken ct)
        {
            var items = await _owners.ListWithPropertiesAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<OwnerDTO>>.Ok(items));
        }

        /// <summary>
        /// Lista todos los owners registrados.
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<OwnerBasicDTO>>>> GetAll(CancellationToken ct)
        {
            var items = await _owners.ListAllAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<OwnerBasicDTO>>.Ok(items));
        }
    }
}

