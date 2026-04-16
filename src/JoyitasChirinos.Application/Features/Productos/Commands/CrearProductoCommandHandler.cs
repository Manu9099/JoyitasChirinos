using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Features.Productos.Commands;
using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;


namespace JoyitasChirinos.Application.Features.Productos.Commands;

public class CrearProductoCommandHandler(IAppDbContext context, IUnitOfWork uow)
    : IRequestHandler<CrearProductoCommand, Guid>
{
    public async Task<Guid> Handle(CrearProductoCommand req, CancellationToken ct)
    {
        var d = req.Datos;

        var codigoExiste = await context.Productos.AnyAsync(p => p.Codigo == d.Codigo.ToUpper(), ct);
        if (codigoExiste)
            throw new InvalidOperationException($"Ya existe un producto con el código '{d.Codigo}'.");

        var categoriaExiste = await context.Categorias.AnyAsync(c => c.Id == d.CategoriaId, ct);
        if (!categoriaExiste)
            throw new NotFoundException(nameof(Categoria), d.CategoriaId);

        var tipo     = Enum.Parse<TipoProducto>(d.Tipo, ignoreCase: true);
        var material = Enum.Parse<MaterialProducto>(d.Material, ignoreCase: true);

        var producto = Producto.Crear(
            d.Codigo, d.Nombre, tipo, material,
            d.PrecioCosto, d.PrecioVenta, d.StockInicial,
            d.CategoriaId, d.PesoGramos, d.ProveedorId, d.StockMinimo);

        await uow.Productos.AddAsync(producto, ct);
        await uow.SaveChangesAsync(ct);

        return producto.Id;
    }
}
