using JoyitasChirinos.Domain.Enums;
namespace JoyitasChirinos.Application.Features.Caja.DTOs;
public record RegistrarMovimientoCajaDto(TipoMovimientoCaja Tipo, decimal Monto, string Motivo, string? Observaciones);