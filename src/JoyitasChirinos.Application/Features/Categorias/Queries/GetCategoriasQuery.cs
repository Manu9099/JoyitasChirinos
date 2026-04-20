using JoyitasChirinos.Application.Features.Categorias.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Categorias.Queries;

public record GetCategoriasQuery(
    string? Busqueda = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedCategoriasResult>;

public record PagedCategoriasResult(
    IReadOnlyList<CategoriaResumenDto> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);