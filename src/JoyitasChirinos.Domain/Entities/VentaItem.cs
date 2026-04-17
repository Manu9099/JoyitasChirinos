using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.ValueObjects;
namespace JoyitasChirinos.Domain.Entities;
public class VentaItem : BaseEntity
{
    public Guid VentaId { get; private set; }
    public Guid ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; } 
    public decimal Subtotal { get; private set; } 
    public Producto? Producto { get; private set; }
    protected VentaItem() { }
    internal static VentaItem Crear(Guid ventaId, Guid productoId, int cantidad, decimal precioUnitario)
        => new() { VentaId = ventaId, ProductoId = productoId, Cantidad = cantidad,
            PrecioUnitario = precioUnitario, Subtotal = precioUnitario * cantidad };
}
