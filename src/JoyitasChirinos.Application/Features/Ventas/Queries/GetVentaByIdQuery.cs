using JoyitasChirinos.Application.Features.Ventas.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Ventas.Queries;

public record GetVentaByIdQuery(Guid Id) : IRequest<VentaDetalleDto>;