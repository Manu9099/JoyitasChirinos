using System.Security.Claims;
using JoyitasChirinos.Application.Features.Encargos.Commands;
using JoyitasChirinos.Application.Features.Encargos.DTOs;
using JoyitasChirinos.Application.Features.Encargos.Queries;
using JoyitasChirinos.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EncargosController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEncargos(
        [FromQuery] string? busqueda,
        [FromQuery] EstadoEncargo? estado,
        [FromQuery] Guid? clienteId,
        [FromQuery] DateTime? fechaEntregaDesde,
        [FromQuery] DateTime? fechaEntregaHasta,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetEncargosQuery(
                busqueda,
                estado,
                clienteId,
                fechaEntregaDesde,
                fechaEntregaHasta,
                pagina,
                tamanoPagina),
            ct);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEncargo(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEncargoByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Crear([FromBody] CrearEncargoDto dto, CancellationToken ct)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value;

        if (!Guid.TryParse(userIdValue, out var usuarioId))
            return Unauthorized(new { mensaje = "No se pudo obtener el usuario autenticado." });

        var id = await mediator.Send(new CrearEncargoCommand(usuarioId, dto), ct);
        return CreatedAtAction(nameof(GetEncargo), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarEncargoDto dto, CancellationToken ct)
    {
        await mediator.Send(new ActualizarEncargoCommand(id, dto), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/estado")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoEncargoDto dto, CancellationToken ct)
    {
        await mediator.Send(new CambiarEstadoEncargoCommand(id, dto.Estado), ct);
        return NoContent();
    }
}