using JoyitasChirinos.Domain.Enums;
using MediatR;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public record CambiarEstadoEncargoCommand(
    Guid Id,
    EstadoEncargo Estado
) : IRequest;