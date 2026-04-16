using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Domain.Interfaces.Services;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using JoyitasChirinos.Domain.Entities;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public class SubirFotoProductoCommandHandler(
    IAppDbContext context, IUnitOfWork uow, IStorageService storage)
    : IRequestHandler<SubirFotoProductoCommand, string>
{
    public async Task<string> Handle(SubirFotoProductoCommand req, CancellationToken ct)
    {
        var producto = await context.Productos.FindAsync([req.ProductoId], ct)
            ?? throw new NotFoundException(nameof(Producto), req.ProductoId);

        var url = await storage.SubirFotoAsync(req.FotoStream, req.NombreArchivo, ct);
        producto.ActualizarFoto(url);

        uow.Productos.Update(producto);
        await uow.SaveChangesAsync(ct);

        return url;
    }
}
