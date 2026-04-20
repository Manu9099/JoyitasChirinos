using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Application.Features.Encargos.DTOs;

public record ActualizarEncargoDto(
    string Descripcion,
    MaterialProducto Material,
    decimal? PesoEstimado,
    decimal PrecioAcordado,
    decimal Adelanto,
    DateTime? FechaEntrega,
    string? FotoReferenciaUrl,
    string? Notas
);