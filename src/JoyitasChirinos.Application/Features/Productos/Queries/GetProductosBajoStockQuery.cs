using JoyitasChirinos.Application.Features.Productos.DTOs;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;


namespace JoyitasChirinos.Application.Features.Productos.Queries;

public record GetProductosBajoStockQuery : IRequest<IReadOnlyList<ProductoResumenDto>>;

public class GetProductosBajoStockQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductosBajoStockQuery, IReadOnlyList<ProductoResumenDto>>
{
    public async Task<IReadOnlyList<ProductoResumenDto>> Handle(GetProductosBajoStockQuery _, CancellationToken ct)
        => await context.Productos
            .AsNoTracking()
            .Where(p => p.StockActual <= p.StockMinimo)
            .OrderBy(p => p.StockActual)
            .Select(p => new ProductoResumenDto(
                p.Id, p.Codigo, p.Nombre,
                p.Tipo.ToString(), p.Material.ToString(),
                p.PrecioVenta.Monto, p.StockActual,
                p.FotoUrl, p.Estado.ToString()))
            .ToListAsync(ct);
}
