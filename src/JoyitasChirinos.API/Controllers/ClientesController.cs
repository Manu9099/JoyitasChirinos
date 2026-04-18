using JoyitasChirinos.Application.Features.Clientes.Commands;
using JoyitasChirinos.Application.Features.Clientes.DTOs;
using JoyitasChirinos.Application.Features.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClientes(
        [FromQuery] string? busqueda,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetClientesQuery(busqueda, pagina, tamanoPagina), ct);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCliente(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetClienteByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Crear([FromBody] CrearClienteDto dto, CancellationToken ct)
    {
        var id = await mediator.Send(new CrearClienteCommand(dto), ct);
        return CreatedAtAction(nameof(GetCliente), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarClienteDto dto,
        CancellationToken ct)
    {
        await mediator.Send(new ActualizarClienteCommand(id, dto), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new EliminarClienteCommand(id), ct);
        return NoContent();
    }
}