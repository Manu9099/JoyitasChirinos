using MediatR;

namespace JoyitasChirinos.Application.Features.Proveedores.Commands;

public record DesactivarProveedorCommand(Guid Id) : IRequest;