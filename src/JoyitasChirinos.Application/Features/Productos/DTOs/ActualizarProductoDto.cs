namespace JoyitasChirinos.Application.Features.Productos.DTOs;

public record ActualizarProductoDto(
    string Nombre,
    string Tipo,
    string Material,
    decimal PrecioCosto,
    decimal PrecioVenta,
    int StockMinimo,
    Guid CategoriaId,
    Guid? ProveedorId,
    decimal? PesoGramos,
    string? Descripcion
);
