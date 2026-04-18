using System.Security.Claims;
using JoyitasChirinos.Application.Features.Ventas.Commands;
using JoyitasChirinos.Application.Features.Ventas.DTOs;
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

    [HttpPost]
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
        return Ok(new { id = ventaId, mensaje = "Venta registrada correctamente" });
    }
}