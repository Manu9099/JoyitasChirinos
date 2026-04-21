using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using JoyitasChirinos.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public class GetCajaSesionByIdQueryHandler : IRequestHandler<GetCajaSesionByIdQuery, CajaSesionDetalleDto> 
{
    private readonly IAppDbContext _context;
    public GetCajaSesionByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<CajaSesionDetalleDto> Handle(GetCajaSesionByIdQuery request, CancellationToken ct) 
    {
        var caja = await _context.CajaSesiones.AsNoTracking().Include(x => x.Movimientos).FirstOrDefaultAsync(x => x.Id == request.Id, ct) ?? throw new NotFoundException("CajaSesion", request.Id);

        decimal ventasEfectivo = 0, ventasYape = 0, ventasPlin = 0, ventasTarjeta = 0, ventasTransferencia = 0, ventasOtros = 0;
        decimal totalIngresosManuales, totalEgresosManuales, totalVentasGeneral, montoEsperado;
        decimal? diferencia;
        string? estadoCierre;

        if (caja.Abierta)
        {
            var ventas = await _context.Ventas.AsNoTracking().Where(v => !v.Anulada && v.Fecha >= caja.FechaApertura).Select(v => new { v.MetodoPago, v.Total }).ToListAsync(ct);
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
            totalIngresosManuales = caja.Movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Ingreso).Sum(m => m.Monto);
            totalEgresosManuales = caja.Movimientos.Where(m => m.Tipo == TipoMovimientoCaja.Egreso).Sum(m => m.Monto);
            totalVentasGeneral = ventasEfectivo + ventasYape + ventasPlin + ventasTarjeta + ventasTransferencia + ventasOtros;
            montoEsperado = caja.MontoInicial + ventasEfectivo + totalIngresosManuales - totalEgresosManuales;
            diferencia = caja.MontoFinalContado.HasValue ? caja.MontoFinalContado.Value - montoEsperado : null;
            estadoCierre = caja.EstadoCierre;
        }
        else
        {
            ventasEfectivo = caja.TotalVentasEfectivoCierre ?? 0m;
            totalVentasGeneral = caja.TotalVentasGeneralCierre ?? 0m;
            totalIngresosManuales = caja.TotalIngresosManualesCierre ?? 0m;
            totalEgresosManuales = caja.TotalEgresosManualesCierre ?? 0m;
            montoEsperado = caja.MontoEsperadoCierre ?? 0m;
            diferencia = caja.DiferenciaCierre;
            estadoCierre = caja.EstadoCierre;
        }

        return new CajaSesionDetalleDto(caja.Id, caja.UsuarioAperturaId, caja.UsuarioCierreId, caja.FechaApertura, caja.FechaCierre, caja.MontoInicial, caja.MontoFinalContado, ventasEfectivo, ventasYape, ventasPlin, ventasTarjeta, ventasTransferencia, ventasOtros, totalVentasGeneral, totalIngresosManuales, totalEgresosManuales, montoEsperado, diferencia, estadoCierre, caja.Abierta, caja.ObservacionesApertura, caja.ObservacionesCierre, caja.Movimientos.OrderByDescending(m => m.FechaMovimiento).Select(m => new CajaMovimientoDto(m.Id, m.Tipo, m.Monto, m.Motivo, m.Observaciones, m.FechaMovimiento, m.UsuarioId)).ToList());
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