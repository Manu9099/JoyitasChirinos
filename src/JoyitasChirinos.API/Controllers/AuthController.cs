using BCrypt.Net;
using JoyitasChirinos.API.Contracts.Auth;
using JoyitasChirinos.Domain.Interfaces.Services;
using JoyitasChirinos.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Activo);

        if (usuario is null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        var passwordOk = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);
        if (!passwordOk)
            return Unauthorized(new { message = "Credenciales inválidas" });

        var token = _tokenService.GenerarToken(usuario);

        return Ok(new LoginResponse
        {
            Token = token,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString()
        });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new
        {
            Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            Nombre = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            Rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}