using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoWasteApi.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ControllerBase
{
  [HttpPost("register")]
  [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
  {
    var response = await authService.RegisterAsync(request, cancellationToken);
    return StatusCode(StatusCodes.Status201Created, response);
  }

  [HttpPost("login")]
  [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
  {
    var response = await authService.LoginAsync(request, cancellationToken);
    return Ok(response);
  }
}

