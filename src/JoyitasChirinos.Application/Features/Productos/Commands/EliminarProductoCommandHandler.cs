using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using JoyitasChirinos.Domain.Entities;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public class EliminarProductoCommandHandler(IAppDbContext context, IUnitOfWork uow)
    : IRequestHandler<EliminarProductoCommand>
{
    public async Task Handle(EliminarProductoCommand req, CancellationToken ct)
    {
        var producto = await context.Productos.FindAsync([req.Id], ct)
            ?? throw new NotFoundException(nameof(Producto), req.Id);

        uow.Productos.Delete(producto);
        await uow.SaveChangesAsync(ct);
    }
}
