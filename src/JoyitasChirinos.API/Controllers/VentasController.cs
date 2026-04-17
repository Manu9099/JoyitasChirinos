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
        var command = new CrearVentaCommand(
            dto.ClienteId,
            dto.Descuento,
            dto.MetodoPago,
            dto.Notas,
            dto.Items
        );

        var ventaId = await _mediator.Send(command, ct);

        return Ok(new
        {
            id = ventaId,
            mensaje = "Venta registrada correctamente"
        });
    }
}