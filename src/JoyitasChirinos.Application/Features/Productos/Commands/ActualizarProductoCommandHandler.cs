using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using JoyitasChirinos.Domain.Entities;


namespace JoyitasChirinos.Application.Features.Productos.Commands;

public class ActualizarProductoCommandHandler(IAppDbContext context, IUnitOfWork uow)
    : IRequestHandler<ActualizarProductoCommand>
{
    public async Task Handle(ActualizarProductoCommand req, CancellationToken ct)
    {
        var producto = await context.Productos.FindAsync([req.Id], ct)
            ?? throw new NotFoundException(nameof(Producto), req.Id);

        var d = req.Datos;
        var tipo     = Enum.Parse<TipoProducto>(d.Tipo, ignoreCase: true);
        var material = Enum.Parse<MaterialProducto>(d.Material, ignoreCase: true);

        // Recreamos via reflection-free: usamos el método interno o EF tracking
        // Para mantener encapsulación del dominio, exponemos un método Actualizar
        // (lo agregaremos a la entidad)
        producto.Actualizar(
            d.Nombre, tipo, material,
            d.PrecioCosto, d.PrecioVenta,
            d.StockMinimo, d.CategoriaId,
            d.ProveedorId, d.PesoGramos, d.Descripcion);

        uow.Productos.Update(producto);
        await uow.SaveChangesAsync(ct);
    }
}
