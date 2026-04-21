using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using JoyitasChirinos.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public class GetCajaActualQueryHandler : IRequestHandler<GetCajaActualQuery, CajaActualDto?> 
{
    private readonly IAppDbContext _context;
    public GetCajaActualQueryHandler(IAppDbContext context) => _context = context;

    public async Task<CajaActualDto?> Handle(GetCajaActualQuery request, CancellationToken ct) 
    {
        var caja = await _context.CajaSesiones.AsNoTracking().Where(x => x.Abierta).OrderByDescending(x => x.FechaApertura).FirstOrDefaultAsync(ct);
        if (caja is null) return null;

        var ventas = await _context.Ventas.AsNoTracking().Where(v => !v.Anulada && v.Fecha >= caja.FechaApertura).Select(v => new { v.MetodoPago, v.Total }).ToListAsync(ct);
        decimal ventasEfectivo = 0, ventasYape = 0, ventasPlin = 0, ventasTarjeta = 0, ventasTransferencia = 0, ventasOtros = 0;
        foreach (var v in ventas)
        {
            switch (NormalizarMetodoPago(v.MetodoPago))
            {
                case "efectivo": ventasEfectivo += v.Total; break;
                case "yape": ventasYape += v.Total; break;
                case "plin": ventasPlin += v.Total; break;
                case "tarjeta": ventasTarjeta += v.Total; break;
                case "transferencia": ventasTransferencia += v.Total; break;
                default: ventasOtros += v.Total; break;
            }
        }

        var movimientos = await _context.CajaMovimientos.AsNoTracking().Where(m => m.CajaSesionId == caja.Id).OrderByDescending(m => m.FechaMovimiento).ToListAsync(ct);
        var totalIngresosManuales = movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Ingreso).Sum(m => m.Monto);
        var totalEgresosManuales = movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Egreso).Sum(m => m.Monto);
        var totalVentasGeneral = ventasEfectivo + ventasYape + ventasPlin + ventasTarjeta + ventasTransferencia + ventasOtros;
        var montoEsperado = caja.MontoInicial + ventasEfectivo + totalIngresosManuales - totalEgresosManuales;

        return new CajaActualDto(caja.Id, caja.UsuarioAperturaId, caja.FechaApertura, caja.MontoInicial, caja.Abierta, caja.ObservacionesApertura, ventasEfectivo, ventasYape, ventasPlin, ventasTarjeta, ventasTransferencia, ventasOtros, totalVentasGeneral, totalIngresosManuales, totalEgresosManuales, montoEsperado, movimientos.Select(m => new CajaMovimientoDto(m.Id, m.Tipo, m.Monto, m.Motivo, m.Observaciones, m.FechaMovimiento, m.UsuarioId)).ToList());
    }

    private static string NormalizarMetodoPago(string? metodoPago) => string.IsNullOrWhiteSpace(metodoPago) ? "otro" : metodoPago.Trim().ToLowerInvariant() switch
    {
        "efectivo" or "cash" => "efectivo",
        "yape" => "yape",
        "plin" => "plin",
        "tarjeta" or "visa" or "mastercard" or "debito" or "credito" => "tarjeta",
        "transferencia" or "transferencia bancaria" => "transferencia",
        _ => "otro"
    };
}