namespace JoyitasChirinos.Application.Features.Clientes.DTOs;

public record ClienteResumenDto(
    Guid Id,
    string Nombre,
    string? Telefono,
    string? Email,
    string? Dni,
    int PuntosFidelidad
);