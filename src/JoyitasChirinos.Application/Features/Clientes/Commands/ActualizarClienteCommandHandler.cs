using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public class ActualizarClienteCommandHandler(
    IAppDbContext context,
    IUnitOfWork uow) : IRequestHandler<ActualizarClienteCommand>
{
    public async Task Handle(ActualizarClienteCommand request, CancellationToken ct)
    {
        var cliente = await context.Clientes.FindAsync([request.Id], ct)
            ?? throw new NotFoundException(nameof(Cliente), request.Id);

        var d = request.Datos;

        if (!string.IsNullOrWhiteSpace(d.Dni))
        {
            var dniExiste = await context.Clientes
                .AnyAsync(x => x.Id != request.Id && x.Dni == d.Dni.Trim(), ct);

            if (dniExiste)
                throw new InvalidOperationException($"Ya existe otro cliente con DNI '{d.Dni}'.");
        }

        if (!string.IsNullOrWhiteSpace(d.Email))
        {
            var emailExiste = await context.Clientes
                .AnyAsync(x => x.Id != request.Id && x.Email == d.Email.Trim(), ct);

            if (emailExiste)
                throw new InvalidOperationException($"Ya existe otro cliente con email '{d.Email}'.");
        }

        cliente.Actualizar(
            d.Nombre,
            d.Telefono,
            d.Email,
            d.Dni,
            d.Notas);

        uow.Clientes.Update(cliente);
        await uow.SaveChangesAsync(ct);
    }
}