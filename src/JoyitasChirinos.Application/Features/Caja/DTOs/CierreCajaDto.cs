namespace JoyitasChirinos.Application.Features.Caja.DTOs;

public record CierreCajaDto(
    decimal MontoFinal,
    string? Observaciones
);