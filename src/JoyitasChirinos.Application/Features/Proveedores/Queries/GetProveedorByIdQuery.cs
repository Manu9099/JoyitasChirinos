using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Proveedores.Queries;

public record GetProveedorByIdQuery(Guid Id) : IRequest<ProveedorDetalleDto>;