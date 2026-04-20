using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Queries;

public class GetProveedoresQueryHandler : IRequestHandler<GetProveedoresQuery, PagedProveedoresResult>
{
    private readonly IAppDbContext _context;

    public GetProveedoresQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedProveedoresResult> Handle(GetProveedoresQuery request, CancellationToken ct)
    {
        var query = _context.Proveedores
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busqueda))
        {
            var b = request.Busqueda.Trim().ToLower();

            query = query.Where(x =>
                x.Nombre.ToLower().Contains(b) ||
                (x.Telefono != null && x.Telefono.ToLower().Contains(b)) ||
                (x.Email != null && x.Email.ToLower().Contains(b)));
        }

        if (!string.IsNullOrWhiteSpace(request.Tipo))
        {
            var tipo = request.Tipo.Trim().ToLower();
            query = query.Where(x => x.Tipo.ToLower() == tipo);
        }

        if (request.Activo.HasValue)
            query = query.Where(x => x.Activo == request.Activo.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(x => x.Nombre)
            .Skip((request.Pagina - 1) * request.TamanoPagina)
            .Take(request.TamanoPagina)
            .Select(x => new ProveedorResumenDto(
                x.Id,
                x.Nombre,
                x.Telefono,
                x.Email,
                x.Tipo,
                x.Activo
            ))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)request.TamanoPagina);

        return new PagedProveedoresResult(
            items,
            total,
            request.Pagina,
            request.TamanoPagina,
            totalPaginas
        );
    }
}