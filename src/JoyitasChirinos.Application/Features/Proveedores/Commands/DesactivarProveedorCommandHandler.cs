using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public class DesactivarProveedorCommandHandler : IRequestHandler<DesactivarProveedorCommand>
{
    private readonly IAppDbContext _context;

    public DesactivarProveedorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DesactivarProveedorCommand request, CancellationToken ct)
    {
        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Proveedor), request.Id);

        proveedor.Desactivar();
        await _context.SaveChangesAsync(ct);
    }
}