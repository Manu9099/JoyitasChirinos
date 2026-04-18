namespace JoyitasChirinos.Application.Features.Ventas.DTOs;

public record VentaItemDto(
    Guid ProductoId,
    string ProductoNombre,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);