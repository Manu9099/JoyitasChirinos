using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public class CrearProveedorCommandHandler : IRequestHandler<CrearProveedorCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CrearProveedorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CrearProveedorCommand request, CancellationToken ct)
    {
        var d = request.Datos;

        var existe = await _context.Proveedores
            .AnyAsync(x => x.Nombre.ToLower() == d.Nombre.Trim().ToLower(), ct);

        if (existe)
            throw new InvalidOperationException($"Ya existe un proveedor con nombre '{d.Nombre}'.");

        var proveedor = Proveedor.Crear(
            d.Nombre,
            d.Telefono,
            d.Email,
            d.Tipo,
            d.Notas
        );

        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync(ct);

        return proveedor.Id;
    }
}