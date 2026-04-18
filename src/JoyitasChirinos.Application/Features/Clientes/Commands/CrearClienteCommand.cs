using JoyitasChirinos.Application.Features.Clientes.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public record CrearClienteCommand(CrearClienteDto Datos) : IRequest<Guid>;