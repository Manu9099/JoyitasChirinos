using JoyitasChirinos.Domain.Common;
using JoyitasChirinos.Domain.Enums;
using JoyitasChirinos.Domain.ValueObjects;
namespace JoyitasChirinos.Domain.Entities;
public class Producto : AuditableEntity
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public TipoProducto Tipo { get; private set; }
    public MaterialProducto Material { get; private set; }
    public PesoGramos? Peso { get; private set; }
    public decimal PrecioCosto { get; private set; } 
    public decimal PrecioVenta { get; private set; } 
    public int StockActual { get; private set; }
    public int StockMinimo { get; private set; } = 1;
    public string? FotoUrl { get; private set; }
    public string? Descripcion { get; private set; }
    public EstadoProducto Estado { get; private set; } = EstadoProducto.Activo;
    public Guid CategoriaId { get; private set; }
    public Guid? ProveedorId { get; private set; }
    public Categoria? Categoria { get; private set; }
    public Proveedor? Proveedor { get; private set; }
    protected Producto() { }

    public static Producto Crear(string codigo, string nombre, TipoProducto tipo, MaterialProducto material,
        decimal precioCosto, decimal precioVenta, int stockInicial, Guid categoriaId,
        decimal? pesoGramos = null, Guid? proveedorId = null, int stockMinimo = 1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(codigo);
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        return new Producto
        {
            Codigo = codigo.ToUpper().Trim(), Nombre = nombre.Trim(),
            Tipo = tipo, Material = material,
            PrecioCosto = precioCosto, PrecioVenta = precioVenta,
            StockActual = stockInicial, StockMinimo = stockMinimo,
            CategoriaId = categoriaId, ProveedorId = proveedorId,
            Peso = pesoGramos.HasValue ? new PesoGramos(pesoGramos.Value) : null
        };
    }

    public void Actualizar(string nombre, TipoProducto tipo, MaterialProducto material,
        decimal precioCosto, decimal precioVenta, int stockMinimo,
        Guid categoriaId, Guid? proveedorId, decimal? pesoGramos, string? descripcion)
    {
        Nombre = nombre.Trim();
        Tipo = tipo; Material = material;
        PrecioCosto = precioCosto;
        PrecioVenta = precioVenta;
        StockMinimo = stockMinimo;
        CategoriaId = categoriaId; ProveedorId = proveedorId;
        Peso = pesoGramos.HasValue ? new PesoGramos(pesoGramos.Value) : null;
        Descripcion = descripcion?.Trim();
        Touch();
    }

    public void AgregarStock(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("Cantidad debe ser positiva");
        StockActual += cantidad;
        if (Estado == EstadoProducto.Agotado) Estado = EstadoProducto.Activo;
        Touch();
    }

    public void RetirarStock(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("Cantidad debe ser positiva");
        if (cantidad > StockActual) throw new InvalidOperationException("Stock insuficiente");
        StockActual -= cantidad;
        if (StockActual == 0) Estado = EstadoProducto.Agotado;
        Touch();
    }

    public bool TieneBajoStock => StockActual <= StockMinimo;
    public void ActualizarFoto(string url) { FotoUrl = url; Touch(); }
}
