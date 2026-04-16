using JoyitasChirinos.Application.Features.Productos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Queries;

public record GetProductosQuery(
    string? Tipo = null,
    string? Material = null,
    string? Estado = null,
    string? Busqueda = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedResult<ProductoResumenDto>>;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);
