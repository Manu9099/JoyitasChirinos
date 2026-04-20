using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Categorias.DTOs;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Categorias.Queries;

public class GetCategoriaByIdQueryHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaDetalleDto>
{
    private readonly IAppDbContext _context;

    public GetCategoriaByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CategoriaDetalleDto> Handle(GetCategoriaByIdQuery request, CancellationToken ct)
    {
        var categoria = await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Categoria), request.Id);

        return new CategoriaDetalleDto(
            categoria.Id,
            categoria.Nombre,
            categoria.Descripcion,
            categoria.CreatedAt
        );
    }
}