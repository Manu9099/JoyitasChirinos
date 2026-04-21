using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using JoyitasChirinos.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public class CerrarCajaCommandHandler : IRequestHandler<CerrarCajaCommand, ResultadoCierreCajaDto> 
{
    private readonly IAppDbContext _context;
    public CerrarCajaCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ResultadoCierreCajaDto> Handle(CerrarCajaCommand request, CancellationToken ct) 
    {
        var caja = await _context.CajaSesiones.FirstOrDefaultAsync(x => x.Abierta, ct);
        if (caja is null) throw new InvalidOperationException("No hay una caja abierta.");

        // Obtener ventas desde apertura
        var ventas = await _context.Ventas.AsNoTracking()
            .Where(v => !v.Anulada && v.Fecha >= caja.FechaApertura)
            .Select(v => new { v.MetodoPago, v.Total })
            .ToListAsync(ct);

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

        var movimientos = await _context.CajaMovimientos.AsNoTracking().Where(m => m.CajaSesionId == caja.Id).ToListAsync(ct);
        var totalIngresosManuales = movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Ingreso).Sum(m => m.Monto);
        var totalEgresosManuales = movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Egreso).Sum(m => m.Monto);
        var totalVentasGeneral = ventasEfectivo + ventasYape + ventasPlin + ventasTarjeta + ventasTransferencia + ventasOtros;
        var montoEsperado = caja.MontoInicial + ventasEfectivo + totalIngresosManuales - totalEgresosManuales;
        var diferencia = request.Datos.MontoFinalContado - montoEsperado;

        string estadoCaja, mensaje;
        if (diferencia > 0) { estadoCaja = "sobrante"; mensaje = $"Hay un sobrante de S/ {diferencia:0.00}."; }
        else if (diferencia < 0) { estadoCaja = "faltante"; mensaje = $"Hay un faltante de S/ {Math.Abs(diferencia):0.00}."; }
        else { estadoCaja = "exacta"; mensaje = "La caja cerró exacta."; }

        caja.Cerrar(request.UsuarioId, request.Datos.MontoFinalContado, ventasEfectivo, totalVentasGeneral, totalIngresosManuales, totalEgresosManuales, montoEsperado, diferencia, estadoCaja, request.Datos.Observaciones);
        await _context.SaveChangesAsync(ct);

        return new ResultadoCierreCajaDto(caja.Id, caja.FechaApertura, caja.FechaCierre!.Value, caja.MontoInicial, ventasEfectivo, ventasYape, ventasPlin, ventasTarjeta, ventasTransferencia, ventasOtros, totalVentasGeneral, totalIngresosManuales, totalEgresosManuales, montoEsperado, request.Datos.MontoFinalContado, diferencia, estadoCaja, mensaje);
    }

    private static string NormalizarMetodoPago(string? metodoPago)
    {
        if (string.IsNullOrWhiteSpace(metodoPago)) return "otro";
        return metodoPago.Trim().ToLowerInvariant() switch
        {
            "efectivo" or "cash" => "efectivo",
            "yape" => "yape",
            "plin" => "plin",
            "tarjeta" or "visa" or "mastercard" or "debito" or "credito" => "tarjeta",
            "transferencia" or "transferencia bancaria" => "transferencia",
            _ => "otro"
        };
    }
}