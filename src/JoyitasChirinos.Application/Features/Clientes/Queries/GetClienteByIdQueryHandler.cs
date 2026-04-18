using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Clientes.DTOs;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Clientes.Queries;

public class GetClienteByIdQueryHandler(
    IAppDbContext context) : IRequestHandler<GetClienteByIdQuery, ClienteDto>
{
    public async Task<ClienteDto> Handle(GetClienteByIdQuery request, CancellationToken ct)
    {
        var cliente = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Cliente), request.Id);

        return new ClienteDto(
            cliente.Id,
            cliente.Nombre,
            cliente.Telefono,
            cliente.Email,
            cliente.Dni,
            cliente.PuntosFidelidad,
            cliente.Notas,
            cliente.CreatedAt);
    }
}