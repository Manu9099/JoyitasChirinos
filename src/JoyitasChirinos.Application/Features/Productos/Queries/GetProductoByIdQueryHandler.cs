using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Features.Productos.DTOs;
using JoyitasChirinos.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using JoyitasChirinos.Domain.Entities;


namespace JoyitasChirinos.Application.Features.Productos.Queries;

public class GetProductoByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetProductoByIdQuery, ProductoDto>
{
    public async Task<ProductoDto> Handle(GetProductoByIdQuery req, CancellationToken ct)
    {
        var p = await context.Productos
            .AsNoTracking()
            .Include(x => x.Categoria)
            .Include(x => x.Proveedor)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct)
            ?? throw new NotFoundException(nameof(Producto), req.Id);

        return new ProductoDto(
            p.Id, p.Codigo, p.Nombre,
            p.Tipo.ToString(), p.Material.ToString(),
            p.Peso?.Valor,
            p.PrecioCosto.Monto, p.PrecioVenta.Monto,
            p.StockActual, p.StockMinimo, p.TieneBajoStock,
            p.FotoUrl, p.Descripcion, p.Estado.ToString(),
            p.CategoriaId, p.Categoria!.Nombre,
            p.ProveedorId, p.Proveedor?.Nombre,
            p.CreatedAt);
    }
}
