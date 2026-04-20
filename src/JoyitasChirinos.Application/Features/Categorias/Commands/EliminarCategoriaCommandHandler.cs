using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Categorias.Commands;

public class EliminarCategoriaCommandHandler : IRequestHandler<EliminarCategoriaCommand>
{
    private readonly IAppDbContext _context;

    public EliminarCategoriaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EliminarCategoriaCommand request, CancellationToken ct)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Categoria), request.Id);

        var tieneProductos = await _context.Productos
            .AnyAsync(p => p.CategoriaId == request.Id, ct);

        if (tieneProductos)
            throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados.");

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync(ct);
    }
}