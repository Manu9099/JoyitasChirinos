using JoyitasChirinos.Application.Features.Ventas.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Ventas.Commands;

public record CrearVentaCommand(
    Guid? ClienteId,
    decimal Descuento,
    string MetodoPago,
    string? Notas,
    List<CrearVentaItemDto> Items
) : IRequest<Guid>;