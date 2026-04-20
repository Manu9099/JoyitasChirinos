namespace JoyitasChirinos.Application.Features.Categorias.DTOs;

public record CategoriaResumenDto(
    Guid Id,
    string Nombre,
    string? Descripcion
);