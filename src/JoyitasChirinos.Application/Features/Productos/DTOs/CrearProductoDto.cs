namespace JoyitasChirinos.Application.Features.Productos.DTOs;

public record CrearProductoDto(
    string Codigo,
    string Nombre,
    string Tipo,
    string Material,
    decimal PrecioCosto,
    decimal PrecioVenta,
    int StockInicial,
    int StockMinimo,
    Guid CategoriaId,
    Guid? ProveedorId,
    decimal? PesoGramos,
    string? Descripcion
);
