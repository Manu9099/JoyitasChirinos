using JoyitasChirinos.Application.Features.Productos.Commands;
using JoyitasChirinos.Application.Features.Productos.DTOs;
using JoyitasChirinos.Application.Features.Productos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyitasChirinos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController(IMediator mediator) : ControllerBase
{
    // GET api/productos?tipo=Anillo&material=Oro18k&pagina=1
    [HttpGet]
    public async Task<IActionResult> GetProductos(
        [FromQuery] string? tipo,
        [FromQuery] string? material,
        [FromQuery] string? estado,
        [FromQuery] string? busqueda,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetProductosQuery(tipo, material, estado, busqueda, pagina, tamanoPagina), ct);
        return Ok(result);
    }

    // GET api/productos/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProducto(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductoByIdQuery(id), ct);
        return Ok(result);
    }

    // GET api/productos/bajo-stock
    [HttpGet("bajo-stock")]
    public async Task<IActionResult> GetBajoStock(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductosBajoStockQuery(), ct);
        return Ok(result);
    }

    // POST api/productos
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto, CancellationToken ct)
    {
        var id = await mediator.Send(new CrearProductoCommand(dto), ct);
        return CreatedAtAction(nameof(GetProducto), new { id }, new { id });
    }

    // PUT api/productos/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Actualizar(
        Guid id, [FromBody] ActualizarProductoDto dto, CancellationToken ct)
    {
        await mediator.Send(new ActualizarProductoCommand(id, dto), ct);
        return NoContent();
    }

    // POST api/productos/{id}/foto
    [HttpPost("{id:guid}/foto")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SubirFoto(Guid id, IFormFile foto, CancellationToken ct)
    {
        if (foto.Length == 0) return BadRequest("Archivo vacío.");
        if (!foto.ContentType.StartsWith("image/"))
            return BadRequest("Solo se permiten imágenes.");

        using var stream = foto.OpenReadStream();
        var url = await mediator.Send(
            new SubirFotoProductoCommand(id, stream, foto.FileName), ct);
        return Ok(new { url });
    }

    // PATCH api/productos/{id}/stock
    [HttpPatch("{id:guid}/stock")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AjustarStock(
        Guid id, [FromBody] AjustarStockRequest req, CancellationToken ct)
    {
        await mediator.Send(new AjustarStockCommand(id, req.Cantidad, req.Operacion), ct);
        return NoContent();
    }

    // DELETE api/productos/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new EliminarProductoCommand(id), ct);
        return NoContent();
    }
}

public record AjustarStockRequest(int Cantidad, string Operacion);
