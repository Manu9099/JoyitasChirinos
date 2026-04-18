using JoyitasChirinos.Application.Features.Ventas.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Ventas.Queries;

public record GetVentasQuery(
    DateTime? Desde = null,
    DateTime? Hasta = null,
    Guid? ClienteId = null,
    string? MetodoPago = null,
    bool? Anulada = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedVentasResult>;

public record PagedVentasResult(
    IReadOnlyList<VentaResumenDto> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);