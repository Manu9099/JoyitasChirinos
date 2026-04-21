namespace JoyitasChirinos.Application.Features.Caja.DTOs;

public record CierreCajaResultadoDto(
    Guid Id,
    DateTime FechaApertura,
    DateTime FechaCierre,
    decimal MontoInicial,
    decimal TotalVentasEfectivo,
    decimal MontoEsperado,
    decimal MontoFinal,
    decimal Diferencia,
    string EstadoCaja,
    string Mensaje
);