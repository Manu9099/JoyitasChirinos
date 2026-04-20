namespace JoyitasChirinos.Application.Features.Categorias.DTOs;

public record CategoriaDetalleDto(
    Guid Id,
    string Nombre,
    string? Descripcion,
    DateTime CreatedAt
);