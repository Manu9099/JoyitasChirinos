using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public class RegistrarMovimientoCajaCommandHandler : IRequestHandler<RegistrarMovimientoCajaCommand, Guid> 
{
    private readonly IAppDbContext _context;
    public RegistrarMovimientoCajaCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Guid> Handle(RegistrarMovimientoCajaCommand request, CancellationToken ct) 
    {
        var caja = await _context.CajaSesiones.FirstOrDefaultAsync(x => x.Abierta, ct);
        if (caja is null) throw new InvalidOperationException("No hay una caja abierta.");

        var movimiento = new CajaMovimiento(caja.Id, request.UsuarioId, request.Datos.Tipo, request.Datos.Monto, request.Datos.Motivo, request.Datos.Observaciones);
        _context.CajaMovimientos.Add(movimiento);
        await _context.SaveChangesAsync(ct);
        return movimiento.Id;
    }
}