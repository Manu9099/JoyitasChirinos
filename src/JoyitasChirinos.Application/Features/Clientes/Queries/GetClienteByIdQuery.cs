using JoyitasChirinos.Application.Features.Clientes.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Clientes.Queries;

public record GetClienteByIdQuery(Guid Id) : IRequest<ClienteDto>;