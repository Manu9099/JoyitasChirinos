using JoyitasChirinos.Domain.Enums;
namespace JoyitasChirinos.Application.Features.Caja.DTOs;
public record CajaMovimientoDto(Guid Id, TipoMovimientoCaja Tipo, decimal Monto, string Motivo, string? Observaciones, DateTime FechaMovimiento, Guid UsuarioId);