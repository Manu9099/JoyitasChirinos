using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public record GetCajaActualQuery : IRequest<CajaActualDto?>;