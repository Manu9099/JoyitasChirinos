using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public class CrearEncargoCommandHandler : IRequestHandler<CrearEncargoCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CrearEncargoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CrearEncargoCommand request, CancellationToken ct)
    {
        var d = request.Datos;

        var clienteExiste = await _context.Clientes
            .AnyAsync(c => c.Id == d.ClienteId, ct);

        if (!clienteExiste)
            throw new InvalidOperationException("El cliente no existe.");

        var ultimoNumero = await _context.Encargos
            .OrderByDescending(x => x.Numero)
            .Select(x => (int?)x.Numero)
            .FirstOrDefaultAsync(ct) ?? 0;

        var encargo = new Encargo(
            d.Numero,
            d.ClienteId,
            request.UsuarioId,
            d.Descripcion,
            d.Material,
            d.PesoEstimado,
            d.PrecioAcordado,
            d.Adelanto,
            d.FechaEntrega,
            d.FotoReferenciaUrl,
            d.Notas
        );

        typeof(Encargo)
            .GetProperty(nameof(Encargo.Numero))!
            .SetValue(encargo, ultimoNumero + 1);

        _context.Encargos.Add(encargo);
        await _context.SaveChangesAsync(ct);

        return encargo.Id;
    }
}