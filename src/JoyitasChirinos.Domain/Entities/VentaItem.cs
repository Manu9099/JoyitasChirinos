using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.ValueObjects;
namespace JoyitasChirinos.Domain.Entities;
public class VentaItem : BaseEntity
{
    public Guid VentaId { get; private set; }
    public Guid ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public Dinero PrecioUnitario { get; private set; } = Dinero.Cero;
    public Dinero Subtotal { get; private set; } = Dinero.Cero;
    public Producto? Producto { get; private set; }
    protected VentaItem() { }
    internal static VentaItem Crear(Guid ventaId, Guid productoId, int cantidad, decimal precioUnitario)
        => new() { VentaId = ventaId, ProductoId = productoId, Cantidad = cantidad,
            PrecioUnitario = new Dinero(precioUnitario), Subtotal = new Dinero(precioUnitario * cantidad) };
}
