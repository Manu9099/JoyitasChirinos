using JoyitasChirinos.Application.Features.Productos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Queries;

public record GetProductoByIdQuery(Guid Id) : IRequest<ProductoDto>;
