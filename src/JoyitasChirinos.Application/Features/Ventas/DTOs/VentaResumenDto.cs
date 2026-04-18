namespace JoyitasChirinos.Application.Features.Ventas.DTOs;

public record VentaResumenDto(
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
    bool Anulada
);