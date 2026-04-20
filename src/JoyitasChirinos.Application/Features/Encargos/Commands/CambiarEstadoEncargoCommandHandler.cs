using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public class CambiarEstadoEncargoCommandHandler : IRequestHandler<CambiarEstadoEncargoCommand>
{
    private readonly IAppDbContext _context;

    public CambiarEstadoEncargoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CambiarEstadoEncargoCommand request, CancellationToken ct)
    {
        var encargo = await _context.Encargos
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Encargo), request.Id);

        encargo.CambiarEstado(request.Estado);
        await _context.SaveChangesAsync(ct);
    }
}