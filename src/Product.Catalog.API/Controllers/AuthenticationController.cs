using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Infra.Authentication;
using System.Security.Claims;

namespace Product.Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost("GenerateToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateTokenAssync([FromBody] AuthenticationModel dto)
        {
            try
            {
                var token = await _authenticationService.GenerateToken(dto);
                
                if (token == null)
                    return Unauthorized();

                return Ok($"Bearer {token}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetCurrentUserInfo"), Authorize]
        public IActionResult GetCurrentUserInfo()
        {
            try
            {
                var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var role = this.User.FindFirst(ClaimTypes.Role)?.Value!;

                return Ok(new { id, role });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}