namespace JoyitasChirinos.Application.Features.Proveedores.DTOs;

public record ProveedorResumenDto(
    Guid Id,
    string Nombre,
    string? Telefono,
    string? Email,
    string Tipo,
    bool Activo
);