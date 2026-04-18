using MediatR;

namespace JoyitasChirinos.Application.Features.Clientes.Commands;

public record EliminarClienteCommand(Guid Id) : IRequest;