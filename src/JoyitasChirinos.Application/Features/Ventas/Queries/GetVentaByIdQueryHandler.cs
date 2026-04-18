using JoyitasChirinos.Application.Common.Exceptions;
using JoyitasChirinos.Application.Common.Interfaces;
using JoyitasChirinos.Application.Features.Ventas.DTOs;
using JoyitasChirinos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Features.Ventas.Queries;

public class GetVentaByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetVentaByIdQuery, VentaDetalleDto>
{
    public async Task<VentaDetalleDto> Handle(GetVentaByIdQuery request, CancellationToken ct)
    {
        var venta = await context.Ventas
            .AsNoTracking()
            .Include(v => v.Cliente)
            .Include(v => v.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(v => v.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Venta), request.Id);

        return new VentaDetalleDto(
            venta.Id,
            venta.Numero,
            venta.Fecha,
            venta.ClienteId,
            venta.Cliente?.Nombre,
            venta.UsuarioId,
            venta.Subtotal,
            venta.Descuento,
            venta.Total,
            venta.MetodoPago,
            venta.Estado,
            venta.Anulada,
            venta.Notas,
            venta.Items.Select(i => new VentaItemDto(
                i.ProductoId,
                i.Producto?.Nombre ?? string.Empty,
                i.Cantidad,
                i.PrecioUnitario,
                i.Subtotal
            )).ToList()
        );
    }
}