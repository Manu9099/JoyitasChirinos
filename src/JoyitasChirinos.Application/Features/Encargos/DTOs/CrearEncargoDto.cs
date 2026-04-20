using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Application.Features.Encargos.DTOs;

public record CrearEncargoDto(
    int Numero,
    Guid ClienteId,
    string Descripcion,
    MaterialProducto Material,
    decimal? PesoEstimado,
    decimal PrecioAcordado,
    decimal Adelanto,
    DateTime? FechaEntrega,
    string? FotoReferenciaUrl,
    string? Notas
);