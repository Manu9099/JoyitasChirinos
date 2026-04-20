using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public class ActualizarProveedorCommandHandler : IRequestHandler<ActualizarProveedorCommand>
{
    private readonly IAppDbContext _context;

    public ActualizarProveedorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActualizarProveedorCommand request, CancellationToken ct)
    {
        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Proveedor), request.Id);

        var d = request.Datos;

        var existe = await _context.Proveedores
            .AnyAsync(x => x.Id != request.Id && x.Nombre.ToLower() == d.Nombre.Trim().ToLower(), ct);

        if (existe)
            throw new InvalidOperationException($"Ya existe otro proveedor con nombre '{d.Nombre}'.");

        proveedor.Actualizar(
            d.Nombre,
            d.Telefono,
            d.Email,
            d.Tipo,
            d.Notas
        );

        await _context.SaveChangesAsync(ct);
    }
}