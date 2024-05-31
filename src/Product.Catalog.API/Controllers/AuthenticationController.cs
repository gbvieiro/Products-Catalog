using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for Authentication.
    /// </summary>
    /// <remarks>
    /// Constroller.
    /// </remarks>
    /// <param name="authenticationService"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        /// <summary>
        /// A authentication service interface.
        /// </summary>
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel userDto)
        {
            try
            {
                var token = await _authenticationService.AuthenticateUser(userDto);

                if (token == null)
                {
                    return Unauthorized();
                }

                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
