using System.Security.Claims;
using JoyitasChirinos.Application.Features.Ventas.Commands;
using JoyitasChirinos.Application.Features.Ventas.DTOs;
using JoyitasChirinos.Application.Features.Ventas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
    private readonly IMediator _mediator;

    public VentasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetVentas(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] Guid? clienteId,
        [FromQuery] string? metodoPago,
        [FromQuery] bool? anulada,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetVentasQuery(desde, hasta, clienteId, metodoPago, anulada, pagina, tamanoPagina), ct);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVenta(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVentaByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Crear([FromBody] CrearVentaDto dto, CancellationToken ct)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value;

        if (!Guid.TryParse(userIdValue, out var usuarioId))
            return Unauthorized(new { mensaje = "No se pudo obtener el usuario autenticado." });

        var command = new CrearVentaCommand(
            usuarioId,
            dto.ClienteId,
            dto.Descuento,
            dto.MetodoPago,
            dto.Notas,
            dto.Items
        );

        var ventaId = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetVenta), new { id = ventaId }, new { id = ventaId });
    }

    [HttpPatch("{id:guid}/anular")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Anular(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new AnularVentaCommand(id), ct);
        return NoContent();
    }
}