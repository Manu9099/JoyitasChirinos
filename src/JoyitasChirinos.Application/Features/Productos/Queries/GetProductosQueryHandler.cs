using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Features.Productos.DTOs;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;


namespace JoyitasChirinos.Application.Features.Productos.Queries;

public class GetProductosQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductosQuery, PagedResult<ProductoResumenDto>>
{
    public async Task<PagedResult<ProductoResumenDto>> Handle(GetProductosQuery req, CancellationToken ct)
    {
        var query = context.Productos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Tipo))
            query = query.Where(p => p.Tipo.ToString() == req.Tipo);

        if (!string.IsNullOrWhiteSpace(req.Material))
            query = query.Where(p => p.Material.ToString() == req.Material);

        if (!string.IsNullOrWhiteSpace(req.Estado))
            query = query.Where(p => p.Estado.ToString() == req.Estado);

        if (!string.IsNullOrWhiteSpace(req.Busqueda))
            query = query.Where(p =>
                p.Nombre.ToLower().Contains(req.Busqueda.ToLower()) ||
                p.Codigo.ToLower().Contains(req.Busqueda.ToLower()));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Nombre)
            .Skip((req.Pagina - 1) * req.TamanoPagina)
            .Take(req.TamanoPagina)
            .Select(p => new ProductoResumenDto(
                p.Id, p.Codigo, p.Nombre,
                p.Tipo.ToString(), p.Material.ToString(),
                p.PrecioVenta, p.StockActual,
                p.FotoUrl, p.Estado.ToString()))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)req.TamanoPagina);

        return new PagedResult<ProductoResumenDto>(items, total, req.Pagina, req.TamanoPagina, totalPaginas);
    }
}
