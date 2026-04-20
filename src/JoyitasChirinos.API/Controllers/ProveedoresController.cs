using JoyitasChirinos.Application.Features.Proveedores.Commands;
using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using JoyitasChirinos.Application.Features.Proveedores.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedoresController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProveedores(
        [FromQuery] string? busqueda,
        [FromQuery] string? tipo,
        [FromQuery] bool? activo,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetProveedoresQuery(busqueda, tipo, activo, pagina, tamanoPagina), ct);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProveedor(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProveedorByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Crear([FromBody] CrearProveedorDto dto, CancellationToken ct)
    {
        var id = await mediator.Send(new CrearProveedorCommand(dto), ct);
        return CreatedAtAction(nameof(GetProveedor), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarProveedorDto dto, CancellationToken ct)
    {
        await mediator.Send(new ActualizarProveedorCommand(id, dto), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new ActivarProveedorCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/desactivar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DesactivarProveedorCommand(id), ct);
        return NoContent();
    }
}