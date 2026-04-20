using JoyitasChirinos.Application.Features.Proveedores.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Proveedores.Queries;

public record GetProveedoresQuery(
    string? Busqueda = null,
    string? Tipo = null,
    bool? Activo = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedProveedoresResult>;

public record PagedProveedoresResult(
    IReadOnlyList<ProveedorResumenDto> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);