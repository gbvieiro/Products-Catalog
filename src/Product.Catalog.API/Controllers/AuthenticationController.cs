using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Infra.Authentication;
using System.Security.Claims;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for Authentication.
    /// </summary>
    /// <param name="authenticationService">A authentication service instance.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        /// <summary>
        /// A authentication service interface.
        /// </summary>
        private readonly IAuthenticationService _authenticationService = authenticationService;

        /// <summary>
        /// Generates a new token.
        /// </summary>
        /// <param name="dto">Authentication model.</param>
        /// <returns>A token.</returns>
        [HttpPost("GenerateToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateTokenAssync([FromBody] AuthenticationModel dto)
        {
            try
            {
                var token = await _authenticationService.AuthenticateUser(dto);
                if (token == null)
                    return Unauthorized();

                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get current user info from token.
        /// </summary>
        /// <returns>Current user information.</returns>
        [HttpGet("GetCurrentUserInfo"), Authorize]
        public IActionResult GetCurrentUserInfo()
        {
            try
            {
                var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var email = this.User!.Identity!.Name;

                return Ok(new { id, email });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}