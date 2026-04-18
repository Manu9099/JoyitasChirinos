using JoyitasChirinos.Application.Features.Clientes.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Clientes.Queries;

public record GetClientesQuery(
    string? Busqueda = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedClientesResult>;

public record PagedClientesResult(
    IReadOnlyList<ClienteResumenDto> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);