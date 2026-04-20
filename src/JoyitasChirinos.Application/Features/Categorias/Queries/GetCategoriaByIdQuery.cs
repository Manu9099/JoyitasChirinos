using JoyitasChirinos.Application.Features.Categorias.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Categorias.Queries;

public record GetCategoriaByIdQuery(Guid Id) : IRequest<CategoriaDetalleDto>;