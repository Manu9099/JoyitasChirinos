using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Clientes.DTOs;
using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public class CrearClienteCommandHandler(
    IAppDbContext context,
    IUnitOfWork uow) : IRequestHandler<CrearClienteCommand, Guid>
{
    public async Task<Guid> Handle(CrearClienteCommand request, CancellationToken ct)
    {
        var d = request.Datos;

        if (!string.IsNullOrWhiteSpace(d.Dni))
        {
            var dniExiste = await context.Clientes
                .AnyAsync(x => x.Dni == d.Dni.Trim(), ct);

            if (dniExiste)
                throw new InvalidOperationException($"Ya existe un cliente con DNI '{d.Dni}'.");
        }

        if (!string.IsNullOrWhiteSpace(d.Email))
        {
            var emailExiste = await context.Clientes
                .AnyAsync(x => x.Email == d.Email.Trim(), ct);

            if (emailExiste)
                throw new InvalidOperationException($"Ya existe un cliente con email '{d.Email}'.");
        }

        var cliente = Cliente.Crear(
            d.Nombre,
            d.Telefono,
            d.Email,
            d.Dni,
            d.Notas);

        await uow.Clientes.AddAsync(cliente, ct);
        await uow.SaveChangesAsync(ct);

        return cliente.Id;
    }
}