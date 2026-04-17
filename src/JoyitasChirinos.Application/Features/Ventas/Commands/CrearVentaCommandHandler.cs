using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Ventas.Commands;

public class CrearVentaCommandHandler : IRequestHandler<CrearVentaCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CrearVentaCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CrearVentaCommand request, CancellationToken ct)
    {
        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("La venta debe tener al menos un item.");

        var venta = new Venta(
            usuarioId: Guid.Parse("11111111-1111-1111-1111-111111111111"), // temporal
            clienteId: request.ClienteId,
            metodoPago: request.MetodoPago,
            notas: request.Notas
        );

        foreach (var item in request.Items)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == item.ProductoId, ct);

            if (producto is null)
                throw new Exception($"No existe el producto {item.ProductoId}");

            if (producto.StockActual < item.Cantidad)
                throw new Exception($"Stock insuficiente para {producto.Nombre}");

            venta.AgregarItem(producto, item.Cantidad);
        }

        if (request.Descuento > 0)
            venta.AplicarDescuento(request.Descuento);

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync(ct);

        return venta.Id;
    }
}