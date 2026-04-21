using JoyitasChirinos.Application.Features.Caja.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Caja.Queries;

public record GetHistorialCajaQuery(DateTime? Desde = null, DateTime? Hasta = null, bool? Abierta = null, int Pagina = 1, int TamanoPagina = 20) : IRequest<PagedCajaSesionResult>;

public record PagedCajaSesionResult(IReadOnlyList<CajaSesionResumenDto> Items, int Total, int Pagina, int TamanoPagina, int TotalPaginas);