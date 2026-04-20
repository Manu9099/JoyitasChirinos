using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Encargos.DTOs;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Encargos.Queries;

public class GetEncargoByIdQueryHandler : IRequestHandler<GetEncargoByIdQuery, EncargoDetalleDto>
{
    private readonly IAppDbContext _context;

    public GetEncargoByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<EncargoDetalleDto> Handle(GetEncargoByIdQuery request, CancellationToken ct)
    {
        var encargo = await _context.Encargos
            .AsNoTracking()
            .Include(x => x.Cliente)
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Encargo), request.Id);

        return new EncargoDetalleDto(
            encargo.Id,
            encargo.Numero,
            encargo.ClienteId,
            encargo.Cliente.Nombre,
            encargo.UsuarioId,
            encargo.Descripcion,
            encargo.Material,
            encargo.PesoEstimado,
            encargo.PrecioAcordado,
            encargo.Adelanto,
            encargo.SaldoPendiente,
            encargo.Estado,
            encargo.FechaEntrega,
            encargo.FotoReferenciaUrl,
            encargo.Notas,
            encargo.CreatedAt,
            encargo.UpdatedAt
        );
    }
}