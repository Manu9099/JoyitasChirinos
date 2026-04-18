using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Ventas.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Ventas.Queries;

public class GetVentasQueryHandler(IAppDbContext context)
    : IRequestHandler<GetVentasQuery, PagedVentasResult>
{
    public async Task<PagedVentasResult> Handle(GetVentasQuery request, CancellationToken ct)
    {
        var query = context.Ventas
            .AsNoTracking()
            .Include(v => v.Cliente)
            .AsQueryable();

        if (request.Desde.HasValue)
            query = query.Where(v => v.Fecha >= request.Desde.Value);

        if (request.Hasta.HasValue)
            query = query.Where(v => v.Fecha <= request.Hasta.Value);

        if (request.ClienteId.HasValue)
            query = query.Where(v => v.ClienteId == request.ClienteId.Value);

        if (!string.IsNullOrWhiteSpace(request.MetodoPago))
            query = query.Where(v => v.MetodoPago == request.MetodoPago);

        if (request.Anulada.HasValue)
            query = query.Where(v => v.Anulada == request.Anulada.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(v => v.Fecha)
            .Skip((request.Pagina - 1) * request.TamanoPagina)
            .Take(request.TamanoPagina)
            .Select(v => new VentaResumenDto(
                v.Id,
                v.Numero,
                v.Fecha,
                v.ClienteId,
                v.Cliente != null ? v.Cliente.Nombre : null,
                v.UsuarioId,
                v.Subtotal,
                v.Descuento,
                v.Total,
                v.MetodoPago,
                v.Estado,
                v.Anulada
            ))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)request.TamanoPagina);

        return new PagedVentasResult(items, total, request.Pagina, request.TamanoPagina, totalPaginas);
    }
}