using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Infra.Authentication;

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
        /// The login method.
        /// </summary>
        /// <param name="dto">Authentication model.</param>
        /// <returns>A token.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthenticationModel dto)
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
    }
}