namespace JoyitasChirinos.Application.Features.Proveedores.DTOs;

public record ActualizarProveedorDto(
    string Nombre,
    string? Telefono,
    string? Email,
    string Tipo,
    string? Notas
);