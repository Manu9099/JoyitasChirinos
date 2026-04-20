using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public class ActualizarEncargoCommandHandler : IRequestHandler<ActualizarEncargoCommand>
{
    private readonly IAppDbContext _context;

    public ActualizarEncargoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActualizarEncargoCommand request, CancellationToken ct)
    {
        var encargo = await _context.Encargos
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Encargo), request.Id);

        var d = request.Datos;

        encargo.Actualizar(
            d.Descripcion,
            d.Material,
            d.PesoEstimado,
            d.PrecioAcordado,
            d.Adelanto,
            d.FechaEntrega,
            d.FotoReferenciaUrl,
            d.Notas
        );

        await _context.SaveChangesAsync(ct);
    }
}