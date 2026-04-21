using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public class CerrarCajaCommandHandler : IRequestHandler<CerrarCajaCommand, CierreCajaResultadoDto>
{
    private readonly IAppDbContext _context;

    public CerrarCajaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CierreCajaResultadoDto> Handle(CerrarCajaCommand request, CancellationToken ct)
    {
        var caja = await _context.CajaSesiones
            .FirstOrDefaultAsync(x => x.Abierta, ct);

        if (caja is null)
            throw new InvalidOperationException("No hay una caja abierta.");

        var fechaApertura = caja.FechaApertura;
        var fechaCierre = DateTime.Now;

        var totalVentasEfectivo = await _context.Ventas
            .Where(v =>
                !v.Anulada &&
                v.Fecha >= fechaApertura &&
                v.Fecha <= fechaCierre &&
                v.MetodoPago.ToLower() == "efectivo")
            .SumAsync(v => (decimal?)v.Total, ct) ?? 0m;

        var montoEsperado = caja.MontoInicial + totalVentasEfectivo;
        var montoFinal = request.Datos.MontoFinal;
        var diferencia = montoFinal - montoEsperado;

        caja.Cerrar(montoFinal, request.Datos.Observaciones);
        await _context.SaveChangesAsync(ct);

        string estadoCaja;
        string mensaje;

        if (diferencia > 0)
        {
            estadoCaja = "sobrante";
            mensaje = $"Hay un sobrante de S/ {diferencia:0.00}.";
        }
        else if (diferencia < 0)
        {
            estadoCaja = "faltante";
            mensaje = $"Hay un faltante de S/ {Math.Abs(diferencia):0.00}.";
        }
        else
        {
            estadoCaja = "exacta";
            mensaje = "La caja cerró exacta.";
        }

        return new CierreCajaResultadoDto(
            caja.Id,
            fechaApertura,
            caja.FechaCierre ?? fechaCierre,
            caja.MontoInicial,
            totalVentasEfectivo,
            montoEsperado,
            montoFinal,
            diferencia,
            estadoCaja,
            mensaje
        );
    }
}