using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public class AbrirCajaCommandHandler : IRequestHandler<AbrirCajaCommand, Guid>
{
    private readonly IAppDbContext _context;

    public AbrirCajaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AbrirCajaCommand request, CancellationToken ct)
    {
        var yaExisteAbierta = await _context.CajaSesiones
            .AnyAsync(x => x.Abierta, ct);

        if (yaExisteAbierta)
            throw new InvalidOperationException("Ya existe una caja abierta.");

        var caja = new CajaSesion(
            request.UsuarioId,
            request.Datos.MontoInicial,
            request.Datos.Observaciones
        );

        _context.CajaSesiones.Add(caja);
        await _context.SaveChangesAsync(ct);

        return caja.Id;
    }
}