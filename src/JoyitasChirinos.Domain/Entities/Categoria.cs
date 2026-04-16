using JoyitasChirinos.Domain.Common;
namespace JoyitasChirinos.Domain.Entities;
public class Categoria : BaseEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    private readonly List<Producto> _productos = [];
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();
    protected Categoria() { }
    public static Categoria Crear(string nombre, string? descripcion = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        return new Categoria { Nombre = nombre.Trim(), Descripcion = descripcion?.Trim() };
    }
    public void Actualizar(string nombre, string? descripcion) { Nombre = nombre.Trim(); Descripcion = descripcion?.Trim(); }
}
