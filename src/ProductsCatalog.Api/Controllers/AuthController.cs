using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Features.Auth;
using ProductsCatalog.Application.Features.Auth.Commands.Login;
using ProductsCatalog.Application.Features.Auth.Commands.Logout;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> LoginAsync([FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// JWT e stateless - ver nota em LogoutCommand. Exige um token valido
    /// (igual qualquer outro endpoint autenticado) so para manter o fluxo
    /// simetrico; a limpeza de verdade acontece no client.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await sender.Send(new LogoutCommand());
        return NoContent();
    }
}
