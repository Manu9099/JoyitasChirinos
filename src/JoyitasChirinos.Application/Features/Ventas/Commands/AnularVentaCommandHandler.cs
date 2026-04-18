using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Ventas.Commands;

public class AnularVentaCommandHandler(IAppDbContext context)
    : IRequestHandler<AnularVentaCommand>
{
    public async Task Handle(AnularVentaCommand request, CancellationToken ct)
    {
        var venta = await context.Ventas
            .Include(v => v.Items)
            .FirstOrDefaultAsync(v => v.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Venta), request.Id);

        if (venta.Anulada)
            throw new InvalidOperationException("La venta ya está anulada.");

        var productoIds = venta.Items.Select(i => i.ProductoId).ToList();

        var productos = await context.Productos
            .Where(p => productoIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, ct);

        foreach (var item in venta.Items)
        {
            if (productos.TryGetValue(item.ProductoId, out var producto))
                producto.AgregarStock(item.Cantidad);
        }

        venta.Anular();
        await context.SaveChangesAsync(ct);
    }
}