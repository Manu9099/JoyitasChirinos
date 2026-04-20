using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Categorias.Commands;

public class ActualizarCategoriaCommandHandler : IRequestHandler<ActualizarCategoriaCommand>
{
    private readonly IAppDbContext _context;

    public ActualizarCategoriaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActualizarCategoriaCommand request, CancellationToken ct)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Categoria), request.Id);

        var d = request.Datos;

        var existe = await _context.Categorias
            .AnyAsync(x => x.Id != request.Id && x.Nombre.ToLower() == d.Nombre.Trim().ToLower(), ct);

        if (existe)
            throw new InvalidOperationException($"Ya existe otra categoría con nombre '{d.Nombre}'.");

        categoria.Actualizar(d.Nombre, d.Descripcion);
        await _context.SaveChangesAsync(ct);
    }
}