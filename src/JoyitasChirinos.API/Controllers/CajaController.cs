using System.Security.Claims;
using JoyitasChirinos.Application.Features.Caja.Commands;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using JoyitasChirinos.Application.Features.Caja.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CajaController : ControllerBase
{
    private readonly IMediator _mediator;

    public CajaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("actual")]
    public async Task<IActionResult> GetActual(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCajaActualQuery(), ct);

        if (result is null)
            return NotFound(new { mensaje = "No hay caja abierta." });

        return Ok(result);
    }

    [HttpPost("apertura")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Apertura([FromBody] AperturaCajaDto dto, CancellationToken ct)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value;

        if (!Guid.TryParse(userIdValue, out var usuarioId))
            return Unauthorized(new { mensaje = "No se pudo obtener el usuario autenticado." });

        var id = await _mediator.Send(new AbrirCajaCommand(usuarioId, dto), ct);
        return Ok(new { id, mensaje = "Caja abierta correctamente." });
    }

[HttpPost("cierre")]
[Authorize(Roles = "Admin,Vendedor")]
public async Task<IActionResult> Cierre([FromBody] CierreCajaDto dto, CancellationToken ct)
{
    var result = await _mediator.Send(new CerrarCajaCommand(dto), ct);
    return Ok(result);
}
}