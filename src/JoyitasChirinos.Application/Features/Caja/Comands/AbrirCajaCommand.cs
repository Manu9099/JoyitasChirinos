using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public record AbrirCajaCommand(
    Guid UsuarioId,
    AperturaCajaDto Datos
) : IRequest<Guid>;