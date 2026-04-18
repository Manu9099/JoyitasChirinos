namespace JoyitasChirinos.Application.Features.Clientes.DTOs;

public record CrearClienteDto(
    string Nombre,
    string? Telefono,
    string? Email,
    string? Dni,
    string? Notas
);