using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.WebApi.Security;

namespace Million.WebApi.Controllers
{
    /// <summary>
    /// Endpoint de prueba para emitir un JWT.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public sealed class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwt;

        public AuthController(JwtTokenService jwt) => _jwt = jwt;

        /// <summary>
        /// <summary>Devuelve un JWT para pruebas.</summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult IssueToken([FromQuery] string user = "system", [FromQuery] string role = "Admin")
        {
            var token = _jwt.CreateToken(Guid.NewGuid().ToString(), user, role);
            return Ok(new { access_token = token, token_type = "Bearer" });
        }
    }
}
