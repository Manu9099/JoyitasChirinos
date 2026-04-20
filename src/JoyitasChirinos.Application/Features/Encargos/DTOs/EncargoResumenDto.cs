using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Application.Features.Encargos.DTOs;

public record EncargoResumenDto(
    Guid Id,
    int Numero,
    Guid ClienteId,
    string? ClienteNombre,
    Guid UsuarioId,
    string Descripcion,
    MaterialProducto Material,
    decimal? PesoEstimado,
    decimal PrecioAcordado,
    decimal Adelanto,
    decimal SaldoPendiente,
    EstadoEncargo Estado,
    DateTime? FechaEntrega
);