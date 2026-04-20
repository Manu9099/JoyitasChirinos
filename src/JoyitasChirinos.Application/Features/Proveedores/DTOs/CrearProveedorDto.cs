namespace JoyitasChirinos.Application.Features.Proveedores.DTOs;

public record CrearProveedorDto(
    string Nombre,
    string? Telefono,
    string? Email,
    string Tipo,
    string? Notas
);