using MediatR;

namespace JoyitasChirinos.Application.Features.Ventas.Commands;

public record AnularVentaCommand(Guid Id) : IRequest;