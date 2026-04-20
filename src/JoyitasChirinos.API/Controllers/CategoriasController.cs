using JoyitasChirinos.Application.Features.Categorias.Commands;
using JoyitasChirinos.Application.Features.Categorias.DTOs;
using JoyitasChirinos.Application.Features.Categorias.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategorias(
        [FromQuery] string? busqueda,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetCategoriasQuery(busqueda, pagina, tamanoPagina), ct);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoria(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoriaByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto, CancellationToken ct)
    {
        var id = await mediator.Send(new CrearCategoriaCommand(dto), ct);
        return CreatedAtAction(nameof(GetCategoria), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarCategoriaDto dto, CancellationToken ct)
    {
        await mediator.Send(new ActualizarCategoriaCommand(id, dto), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new EliminarCategoriaCommand(id), ct);
        return NoContent();
    }
}