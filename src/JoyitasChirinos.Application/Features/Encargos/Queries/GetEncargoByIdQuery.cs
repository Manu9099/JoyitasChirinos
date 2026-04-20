using JoyitasChirinos.Application.Features.Encargos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Encargos.Queries;

public record GetEncargoByIdQuery(Guid Id) : IRequest<EncargoDetalleDto>;