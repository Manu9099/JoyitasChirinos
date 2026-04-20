using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Categorias.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Categorias.Queries;

public class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, PagedCategoriasResult>
{
    private readonly IAppDbContext _context;

    public GetCategoriasQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedCategoriasResult> Handle(GetCategoriasQuery request, CancellationToken ct)
    {
        var query = _context.Categorias
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busqueda))
        {
            var b = request.Busqueda.Trim().ToLower();

            query = query.Where(x =>
                x.Nombre.ToLower().Contains(b) ||
                (x.Descripcion != null && x.Descripcion.ToLower().Contains(b)));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(x => x.Nombre)
            .Skip((request.Pagina - 1) * request.TamanoPagina)
            .Take(request.TamanoPagina)
            .Select(x => new CategoriaResumenDto(
                x.Id,
                x.Nombre,
                x.Descripcion
            ))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)request.TamanoPagina);

        return new PagedCategoriasResult(
            items,
            total,
            request.Pagina,
            request.TamanoPagina,
            totalPaginas
        );
    }
}