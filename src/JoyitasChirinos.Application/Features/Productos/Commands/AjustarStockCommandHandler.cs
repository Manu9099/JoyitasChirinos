using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using JoyitasChirinos.Domain.Entities;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public class AjustarStockCommandHandler(IAppDbContext context, IUnitOfWork uow)
    : IRequestHandler<AjustarStockCommand>
{
    public async Task Handle(AjustarStockCommand req, CancellationToken ct)
    {
        var producto = await context.Productos.FindAsync([req.ProductoId], ct)
            ?? throw new NotFoundException(nameof(Producto), req.ProductoId);

        if (req.Operacion == "agregar")
            producto.AgregarStock(req.Cantidad);
        else if (req.Operacion == "retirar")
            producto.RetirarStock(req.Cantidad);
        else
            throw new ArgumentException("Operación inválida. Use 'agregar' o 'retirar'.");

        uow.Productos.Update(producto);
        await uow.SaveChangesAsync(ct);
    }
}
