using JoyitasChirinos.Domain.Entities;
namespace JoyitasChirinos.Domain.Interfaces.Repositories;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Producto> Productos { get; }
    IGenericRepository<Categoria> Categorias { get; }
    IGenericRepository<Proveedor> Proveedores { get; }
    IGenericRepository<Cliente> Clientes { get; }
    IGenericRepository<Venta> Ventas { get; }
    IGenericRepository<Encargo> Encargos { get; }
    IGenericRepository<Usuario> Usuarios { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
