using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Proveedores.Queries;

public class GetProveedorByIdQueryHandler : IRequestHandler<GetProveedorByIdQuery, ProveedorDetalleDto>
{
    private readonly IAppDbContext _context;

    public GetProveedorByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ProveedorDetalleDto> Handle(GetProveedorByIdQuery request, CancellationToken ct)
    {
        var proveedor = await _context.Proveedores
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Proveedor), request.Id);

        return new ProveedorDetalleDto(
            proveedor.Id,
            proveedor.Nombre,
            proveedor.Telefono,
            proveedor.Email,
            proveedor.Tipo,
            proveedor.Notas,
            proveedor.Activo,
            proveedor.CreatedAt
        );
    }
}