using MediatR;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public record ActivarProveedorCommand(Guid Id) : IRequest;