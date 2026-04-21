using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Caja.Commands;

public record CerrarCajaCommand(
    CierreCajaDto Datos
) : IRequest<CierreCajaResultadoDto>;