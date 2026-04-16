using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public record EliminarProductoCommand(Guid Id) : IRequest;
