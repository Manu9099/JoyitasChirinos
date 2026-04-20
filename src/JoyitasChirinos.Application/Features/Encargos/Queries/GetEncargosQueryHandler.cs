using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Encargos.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Encargos.Queries;

public class GetEncargosQueryHandler : IRequestHandler<GetEncargosQuery, PagedEncargosResult>
{
    private readonly IAppDbContext _context;

    public GetEncargosQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedEncargosResult> Handle(GetEncargosQuery request, CancellationToken ct)
    {
        var query = _context.Encargos
            .AsNoTracking()
            .Include(x => x.Cliente)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busqueda))
        {
            var b = request.Busqueda.Trim().ToLower();
            query = query.Where(x =>
                x.Descripcion.ToLower().Contains(b) ||
                x.Cliente.Nombre.ToLower().Contains(b));
        }

        if (request.Estado.HasValue)
            query = query.Where(x => x.Estado == request.Estado.Value);

        if (request.ClienteId.HasValue)
            query = query.Where(x => x.ClienteId == request.ClienteId.Value);

        if (request.FechaEntregaDesde.HasValue)
            query = query.Where(x => x.FechaEntrega >= request.FechaEntregaDesde.Value);

        if (request.FechaEntregaHasta.HasValue)
            query = query.Where(x => x.FechaEntrega <= request.FechaEntregaHasta.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Pagina - 1) * request.TamanoPagina)
            .Take(request.TamanoPagina)
            .Select(x => new EncargoResumenDto(
                x.Id,
                x.Numero,
                x.ClienteId,
                x.Cliente.Nombre,
                x.UsuarioId,
                x.Descripcion,
                x.Material,
                x.PesoEstimado,
                x.PrecioAcordado,
                x.Adelanto,
                x.SaldoPendiente,
                x.Estado,
                x.FechaEntrega
            ))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)request.TamanoPagina);

        return new PagedEncargosResult(items, total, request.Pagina, request.TamanoPagina, totalPaginas);
    }
}