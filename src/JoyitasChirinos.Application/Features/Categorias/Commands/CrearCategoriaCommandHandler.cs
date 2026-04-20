using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Categorias.Commands;

public class CrearCategoriaCommandHandler : IRequestHandler<CrearCategoriaCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CrearCategoriaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CrearCategoriaCommand request, CancellationToken ct)
    {
        var d = request.Datos;

        var existe = await _context.Categorias
            .AnyAsync(x => x.Nombre.ToLower() == d.Nombre.Trim().ToLower(), ct);

        if (existe)
            throw new InvalidOperationException($"Ya existe una categoría con nombre '{d.Nombre}'.");

        var categoria = Categoria.Crear(d.Nombre, d.Descripcion);

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync(ct);

        return categoria.Id;
    }
}