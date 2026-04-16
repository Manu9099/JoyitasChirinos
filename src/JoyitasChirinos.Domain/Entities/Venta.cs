using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Domain.ValueObjects;
namespace JoyitasChirinos.Domain.Entities;
public class Venta : BaseEntity
{
    public int Numero { get; private set; }
    public Guid? ClienteId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Dinero Subtotal { get; private set; } = Dinero.Cero;
    public Dinero Descuento { get; private set; } = Dinero.Cero;
    public Dinero Total { get; private set; } = Dinero.Cero;
    public MetodoPago MetodoPago { get; private set; }
    public bool Anulada { get; private set; }
    public string? Notas { get; private set; }
    public DateTime Fecha { get; private set; } = DateTime.UtcNow;
    private readonly List<VentaItem> _items = [];
    public IReadOnlyCollection<VentaItem> Items => _items.AsReadOnly();
    public Cliente? Cliente { get; private set; }
    protected Venta() { }
    public static Venta Crear(Guid usuarioId, MetodoPago metodoPago, Guid? clienteId = null, string? notas = null)
        => new() { UsuarioId = usuarioId, MetodoPago = metodoPago, ClienteId = clienteId, Notas = notas };
    public void AgregarItem(Producto producto, int cantidad)
    {
        if (Anulada) throw new InvalidOperationException("Venta anulada");
        producto.RetirarStock(cantidad);
        _items.Add(VentaItem.Crear(Id, producto.Id, cantidad, producto.PrecioVenta.Monto));
        Recalcular();
    }
    public void AplicarDescuento(decimal monto)
    {
        if (monto < 0 || monto > Subtotal.Monto) throw new ArgumentException("Descuento inválido");
        Descuento = new Dinero(monto); Recalcular();
    }
    public void Anular() => Anulada = true;
    private void Recalcular()
    {
        Subtotal = new Dinero(_items.Sum(i => i.Subtotal.Monto));
        Total = new Dinero(Subtotal.Monto - Descuento.Monto);
    }
}
