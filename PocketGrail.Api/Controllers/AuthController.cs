namespace PocketGrail.Api.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Api.Helpers;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var email = await _authService.RegisterAsync(request, ct);
        return Ok(new PendingVerificationResponse { Email = email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var email = await _authService.LoginAsync(request, ct);
        return Ok(new PendingVerificationResponse { Email = email });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyCodeRequest request, CancellationToken ct)
    {
        var token = await _authService.VerifyCodeAsync(request, ct);
        CookieHelper.AppendAuthCookie(Response, token);
        return Ok(new AuthResponse());
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId   = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var role     = User.FindFirstValue(ClaimTypes.Role);
        return Ok(new { userId, username, role });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        CookieHelper.DeleteAuthCookie(Response);
        return Ok(new { message = "Logged out successfully." });
    }
}
