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
    public CajaController(IMediator mediator) => _mediator = mediator;

    [HttpPost("apertura")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Apertura([FromBody] AperturaCajaDto dto, CancellationToken ct) 
    {
        var usuarioId = ObtenerUsuarioId();
        var id = await _mediator.Send(new AbrirCajaCommand(usuarioId, dto), ct);
        return Ok(new { id, mensaje = "Caja abierta correctamente." });
    }

    [HttpGet("actual")]
    public async Task<IActionResult> GetActual(CancellationToken ct) 
    {
        var result = await _mediator.Send(new GetCajaActualQuery(), ct);
        if (result is null) return NotFound(new { mensaje = "No hay caja abierta." });
        return Ok(result);
    }

    [HttpPost("movimientos")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] RegistrarMovimientoCajaDto dto, CancellationToken ct) 
    {
        var usuarioId = ObtenerUsuarioId();
        var id = await _mediator.Send(new RegistrarMovimientoCajaCommand(usuarioId, dto), ct);
        return Ok(new { id, mensaje = "Movimiento registrado correctamente." });
    }

    [HttpPost("cierre")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Cierre([FromBody] CierreCajaDto dto, CancellationToken ct) 
    {
        var usuarioId = ObtenerUsuarioId();
        var result = await _mediator.Send(new CerrarCajaCommand(usuarioId, dto), ct);
        return Ok(result);
    }

    [HttpGet("historial")]
    public async Task<IActionResult> Historial([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] bool? abierta, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken ct = default) 
    {
        var result = await _mediator.Send(new GetHistorialCajaQuery(desde, hasta, abierta, pagina, tamanoPagina), ct);
        return Ok(result);
    }

    [HttpGet("historial/{id:guid}")]
    public async Task<IActionResult> GetSesion(Guid id, CancellationToken ct) 
    {
        var result = await _mediator.Send(new GetCajaSesionByIdQuery(id), ct);
        return Ok(result);
    }

    private Guid ObtenerUsuarioId() 
    {
        var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
        if (!Guid.TryParse(userIdValue, out var usuarioId)) throw new UnauthorizedAccessException("No se pudo obtener el usuario autenticado.");
        return usuarioId;
    }
}