namespace JoyitasChirinos.Application.Features.Caja.DTOs;

public record CajaActualDto(
    Guid Id,
    Guid UsuarioId,
    DateTime FechaApertura,
    decimal MontoInicial,
    bool Abierta,
    string? ObservacionesApertura,
    decimal TotalVentas
);