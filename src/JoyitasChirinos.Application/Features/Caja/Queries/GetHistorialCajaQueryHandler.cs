using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public class GetHistorialCajaQueryHandler : IRequestHandler<GetHistorialCajaQuery, PagedCajaSesionResult> 
{
    private readonly IAppDbContext _context;
    public GetHistorialCajaQueryHandler(IAppDbContext context) => _context = context;

    public async Task<PagedCajaSesionResult> Handle(GetHistorialCajaQuery request, CancellationToken ct) 
    {
        var query = _context.CajaSesiones.AsNoTracking().AsQueryable();
        if (request.Desde.HasValue) query = query.Where(x => x.FechaApertura >= request.Desde.Value);
        if (request.Hasta.HasValue) query = query.Where(x => x.FechaApertura <= request.Hasta.Value);
        if (request.Abierta.HasValue) query = query.Where(x => x.Abierta == request.Abierta.Value);

        var total = await query.CountAsync(ct);
        var items = await query.OrderByDescending(x => x.FechaApertura).Skip((request.Pagina - 1) * request.TamanoPagina).Take(request.TamanoPagina).Select(x => new CajaSesionResumenDto(x.Id, x.FechaApertura, x.FechaCierre, x.UsuarioAperturaId, x.UsuarioCierreId, x.MontoInicial, x.MontoFinalContado, x.MontoEsperadoCierre, x.DiferenciaCierre, x.EstadoCierre, x.Abierta)).ToListAsync(ct);
        
        return new PagedCajaSesionResult(items, total, request.Pagina, request.TamanoPagina, (int)Math.Ceiling(total / (double)request.TamanoPagina));
    }
}