using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Clientes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Clientes.Queries;

public class GetClientesQueryHandler(
    IAppDbContext context) : IRequestHandler<GetClientesQuery, PagedClientesResult>
{
    public async Task<PagedClientesResult> Handle(GetClientesQuery request, CancellationToken ct)
    {
        var query = context.Clientes
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Busqueda))
        {
            var b = request.Busqueda.Trim().ToLower();

            query = query.Where(x =>
                x.Nombre.ToLower().Contains(b) ||
                (x.Telefono != null && x.Telefono.ToLower().Contains(b)) ||
                (x.Email != null && x.Email.ToLower().Contains(b)) ||
                (x.Dni != null && x.Dni.ToLower().Contains(b)));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(x => x.Nombre)
            .Skip((request.Pagina - 1) * request.TamanoPagina)
            .Take(request.TamanoPagina)
            .Select(x => new ClienteResumenDto(
                x.Id,
                x.Nombre,
                x.Telefono,
                x.Email,
                x.Dni,
                x.PuntosFidelidad))
            .ToListAsync(ct);

        var totalPaginas = (int)Math.Ceiling(total / (double)request.TamanoPagina);

        return new PagedClientesResult(
            items,
            total,
            request.Pagina,
            request.TamanoPagina,
            totalPaginas);
    }
}