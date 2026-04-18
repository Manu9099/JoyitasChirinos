namespace JoyitasChirinos.Application.Features.Clientes.DTOs;

public record ClienteDto(
    Guid Id,
    string Nombre,
    string? Telefono,
    string? Email,
    string? Dni,
    int PuntosFidelidad,
    string? Notas,
    DateTime CreatedAt
);