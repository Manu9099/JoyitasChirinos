using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public class GetCajaActualQueryHandler : IRequestHandler<GetCajaActualQuery, CajaActualDto?>
{
    private readonly IAppDbContext _context;

    public GetCajaActualQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CajaActualDto?> Handle(GetCajaActualQuery request, CancellationToken ct)
    {
        var caja = await _context.CajaSesiones
            .AsNoTracking()
            .Where(x => x.Abierta)
            .OrderByDescending(x => x.FechaApertura)
            .FirstOrDefaultAsync(ct);

        if (caja is null)
            return null;

        var totalVentas = await _context.Ventas
            .Where(v => !v.Anulada && v.Fecha >= caja.FechaApertura)
            .SumAsync(v => (decimal?)v.Total, ct) ?? 0m;

        return new CajaActualDto(
            caja.Id,
            caja.UsuarioId,
            caja.FechaApertura,
            caja.MontoInicial,
            caja.Abierta,
            caja.ObservacionesApertura,
            totalVentas
        );
    }
}