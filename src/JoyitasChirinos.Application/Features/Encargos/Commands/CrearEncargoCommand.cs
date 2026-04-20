using JoyitasChirinos.Application.Features.Encargos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Encargos.Commands;

public record CrearEncargoCommand(
    Guid UsuarioId,
    CrearEncargoDto Datos
) : IRequest<Guid>;