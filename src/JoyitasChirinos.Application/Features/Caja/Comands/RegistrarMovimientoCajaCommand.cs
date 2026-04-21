using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;
namespace JoyitasChirinos.Application.Features.Caja.Commands;
public record RegistrarMovimientoCajaCommand(Guid UsuarioId, RegistrarMovimientoCajaDto Datos) : IRequest<Guid>;