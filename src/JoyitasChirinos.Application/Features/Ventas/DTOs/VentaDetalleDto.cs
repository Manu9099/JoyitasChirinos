namespace JoyitasChirinos.Application.Features.Ventas.DTOs;

public record VentaDetalleDto(
    Guid Id,
    int Numero,
    DateTime Fecha,
    Guid? ClienteId,
    string? ClienteNombre,
    Guid UsuarioId,
    decimal Subtotal,
    decimal Descuento,
    decimal Total,
    string MetodoPago,
    string Estado,
    bool Anulada,
    string? Notas,
    IReadOnlyList<VentaItemDto> Items
);