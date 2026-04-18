namespace JoyitasChirinos.Application.Features.Clientes.DTOs;

public record ActualizarClienteDto(
    string Nombre,
    string? Telefono,
    string? Email,
    string? Dni,
    string? Notas
);