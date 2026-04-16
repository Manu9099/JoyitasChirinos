namespace JoyitasChirinos.Application.Features.Productos.DTOs;

public record ProductoDto(
    Guid Id,
    string Codigo,
    string Nombre,
    string Tipo,
    string Material,
    decimal? PesoGramos,
    decimal PrecioCosto,
    decimal PrecioVenta,
    int StockActual,
    int StockMinimo,
    bool TieneBajoStock,
    string? FotoUrl,
    string? Descripcion,
    string Estado,
    Guid CategoriaId,
    string CategoriaNombre,
    Guid? ProveedorId,
    string? ProveedorNombre,
    DateTime CreatedAt
);
