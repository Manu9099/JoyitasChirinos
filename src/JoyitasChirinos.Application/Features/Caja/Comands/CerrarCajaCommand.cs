using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;
namespace JoyitasChirinos.Application.Features.Caja.Commands;
public record CerrarCajaCommand(Guid UsuarioId, CierreCajaDto Datos) : IRequest<ResultadoCierreCajaDto>;