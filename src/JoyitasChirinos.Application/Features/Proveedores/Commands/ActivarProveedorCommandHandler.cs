using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public class ActivarProveedorCommandHandler : IRequestHandler<ActivarProveedorCommand>
{
    private readonly IAppDbContext _context;

    public ActivarProveedorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivarProveedorCommand request, CancellationToken ct)
    {
        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Proveedor), request.Id);

        proveedor.Activar();
        await _context.SaveChangesAsync(ct);
    }
}