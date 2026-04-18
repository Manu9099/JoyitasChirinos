using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public class EliminarClienteCommandHandler(
    IAppDbContext context,
    IUnitOfWork uow) : IRequestHandler<EliminarClienteCommand>
{
    public async Task Handle(EliminarClienteCommand request, CancellationToken ct)
    {
        var cliente = await context.Clientes.FindAsync([request.Id], ct)
            ?? throw new NotFoundException(nameof(Cliente), request.Id);

        var tieneVentas = await context.Ventas
            .AnyAsync(v => v.ClienteId == request.Id, ct);

        if (tieneVentas)
            throw new InvalidOperationException("No se puede eliminar el cliente porque tiene ventas asociadas.");

        uow.Clientes.Delete(cliente);
        await uow.SaveChangesAsync(ct);
    }
}