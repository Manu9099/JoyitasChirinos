namespace JoyitasChirinos.Application.Features.Proveedores.DTOs;

public record ProveedorDetalleDto(
    Guid Id,
    string Nombre,
    string? Telefono,
    string? Email,
    string Tipo,
    string? Notas,
    bool Activo,
    DateTime CreatedAt
);