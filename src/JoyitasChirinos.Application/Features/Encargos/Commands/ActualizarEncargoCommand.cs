using JoyitasChirinos.Application.Features.Encargos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public record ActualizarEncargoCommand(
    Guid Id,
    ActualizarEncargoDto Datos
) : IRequest;