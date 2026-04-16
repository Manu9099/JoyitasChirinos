namespace JoyitasChirinos.Application.Features.Productos.DTOs;

public record ProductoResumenDto(
    Guid Id,
    string Codigo,
    string Nombre,
    string Tipo,
    string Material,
    decimal PrecioVenta,
    int StockActual,
    string? FotoUrl,
    string Estado
);
