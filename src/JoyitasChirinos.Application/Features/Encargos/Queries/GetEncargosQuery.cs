using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Application.Features.Encargos.DTOs;
using MediatR;

namespace JoyitasChirinos.Application.Features.Encargos.Queries;

public record GetEncargosQuery(
    string? Busqueda = null,
    EstadoEncargo? Estado = null,
    Guid? ClienteId = null,
    DateTime? FechaEntregaDesde = null,
    DateTime? FechaEntregaHasta = null,
    int Pagina = 1,
    int TamanoPagina = 20
) : IRequest<PagedEncargosResult>;

public record PagedEncargosResult(
    IReadOnlyList<EncargoResumenDto> Items,
    int Total,
    int Pagina,
    int TamanoPagina,
    int TotalPaginas
);